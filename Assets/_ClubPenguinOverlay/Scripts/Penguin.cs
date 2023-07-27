using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PenguinState
{
    None,
    Idle,
    Walking,
    Sitting,
    Waving,
    Throwing,
    Dancing
}

public class Penguin : Entity
{
    [SerializeField] private PenguinState penguinState;
    [SerializeField] private PlayerData data;

    [SerializeField] private bool isAI;
    public bool IsAI => isAI;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image colorImage;

    [SerializeField] private Bubble bubbleMessage;
    [SerializeField] PenguinManager manager;

    [SerializeField] private float walkingSpeed = 100;
    public float WalkingSpeed => walkingSpeed;

    [SerializeField] private Vector2 updateRate = new Vector2(0.2f, 7f);
    private float updateTime;
    private float timeOutTime;

    [SerializeField] private float aiTimeout = 60;
    private float aiTimeOutTime;

    private Vector2 lastPos;
    //Rotation
    [SerializeField] int horizontalDir = 0;
    [SerializeField] int verticalDir = 0;
    public int HorizontalDir => horizontalDir;
    public int VerticalDir => verticalDir;

    //Throw
    private Vector2 throwTarget;
    [SerializeField] private RectTransform throwPivotDown;
    [SerializeField] private RectTransform throwPivotUp;

    public delegate void PenguinStateDelegate(PenguinState state);
    public event PenguinStateDelegate EventChangeState;

    public delegate void PenguinLookAtDelegate(int posX, int posY);
    public event PenguinLookAtDelegate EventLookAt;

    #region StateMachine
    PenguinBaseState currentState;

    PenguinIdleState IdleState = new PenguinIdleState();
    PenguinWalkingState WalkingState = new PenguinWalkingState();
    PenguinSittingState SittingState = new PenguinSittingState();
    PenguinWavingState WavingState = new PenguinWavingState();
    PenguinThrowingState ThrowingState = new PenguinThrowingState();
    PenguinDancingState DancingState = new PenguinDancingState();


    [SerializeField] public State IdleStateData;
    [SerializeField] public State SitStateData;
    [SerializeField] public State DanceStateData;
    #endregion

    private void OnEnable()
    {
        SetInitialReferences();
    }

    private void SetInitialReferences()
    {
        manager = FindObjectOfType<PenguinManager>();

        RectTransform.anchoredPosition = GetRandomScreenPos();

        timeOutTime = Time.time + ConfigManager.Instance.Config.PenguinTimeout;

        isAI = true;

        LookAtDir(Vector2.down);

        ChangePenguinState(PenguinState.Idle);
    }

    public void UpdateData(PlayerData newData)
    {
        data = newData;

        string icon = "";

        if (ConfigManager.Instance.Config.ShowBadges)
        {
            if (newData.IsSubscriber)
            {
                nameText.alignment = TextAlignmentOptions.Left;
                icon = "<sprite index=0>";
            }
            if (newData.IsMod)
            {
                nameText.alignment = TextAlignmentOptions.Left;
                icon = "<sprite index=1>";
            }
        }
        nameText.SetText(icon + data.Name);
        nameText.color = data.Color;
        colorImage.color = data.Color;

        if(!string.IsNullOrEmpty(newData.EmoteId))
        {
            bubbleMessage.SayEmote(newData.EmoteId);
        }
        else
        {
            string message = data.Message.ToLower();

            if (message.StartsWith("!emote"))
            {
                bubbleMessage.Say(message);
            }
            else if (message.StartsWith("!"))
            {
                isAI = false;
                aiTimeOutTime = Time.time + aiTimeout;
                SendCommand(message);
            }
            else
            {
                bubbleMessage.Say(data.Message);//Send original Message
            }
        }
        timeOutTime = Time.time + ConfigManager.Instance.Config.PenguinTimeout;
    }

    private void SendCommand(string command)
    {
        isAI = false;
        string[] lines = command.Split(" ");

        if (lines[0] == "!move" || lines[0] == "!walk")
        {
            MoveCommand(lines);
        }
        else if (lines[0] == "!sit")
        {
            SitCommand(lines);
        }
        else if (lines[0] == "!dance")
        {
            ChangePenguinState(PenguinState.Dancing);
        }
        else if (lines[0] == "!throw")
        {
            ThrowCommand(lines);
        }
        else if (lines[0] == "!wave")
        {
            ChangePenguinState(PenguinState.Waving);
        }
        else if (lines[0] == "!afk")
        {
            isAI = true;
        }
    }

    private void MoveCommand(string[] prms)
    {
        if (prms.Length <= 1)
        {
            MoveToRandomPos();
        }
        else if (prms.Length <= 2)
        {
            string secondCmd = prms[prms.Length - 1];
            if (float.TryParse(secondCmd, out float dist))
            {
                dist = Mathf.Abs(dist);
                if (dist > 5)
                {
                    dist = 5;
                }
                Vector2 rndm = RectTransform.anchoredPosition + GetRandomScreenPos().normalized * dist * walkingSpeed;
                MoveToPos(rndm);
            }
            else
            {
                Vector2 dir = GetDirWithCommand(secondCmd);
                if (dir != Vector2.zero)
                {
                    Vector2 target = RectTransform.anchoredPosition + dir.normalized * 1 * walkingSpeed;
                    MoveToPos(target);
                }
            }
        }
        else if (prms.Length <= 3)
        {
            string secondCmd = prms[prms.Length - 2];
            string thirdCmd = prms[prms.Length - 1];

            Vector2 dir = GetDirWithCommand(secondCmd);
            if (dir != Vector2.zero)
            {
                if (float.TryParse(thirdCmd, out float dist))
                {
                    dist = Mathf.Abs(dist);
                    if (dist > 5)
                    {
                        dist = 5;
                    }
                    Vector2 target = RectTransform.anchoredPosition + dir.normalized * dist * walkingSpeed;
                    MoveToPos(target);
                }
            }
        }
    }

    private void SitCommand(string[] prms)
    {
        if (prms.Length <= 1)
        {
            SitToDir(new Vector2(horizontalDir, verticalDir));
        }
        else if (prms.Length <= 2)
        {
            string secondCmd = prms[prms.Length - 1];
            Vector2 dir = GetDirWithCommand(secondCmd);
            SitToDir(dir);
        }
    }

    private void ThrowCommand(string[] prms)
    {
        if (prms.Length <= 1)
        {
            ThrowAtRandom();
        }
        else if (prms.Length <= 2) //Penguin Name
        {
            string penguinName = prms[prms.Length - 1];
            ThrowToPenguin(penguinName);
        }
        else if (prms.Length <= 3) //Coordinates
        {
            string xPosCmnd = prms[prms.Length - 2];
            string yPosCmnd = prms[prms.Length - 1];

            if (int.TryParse(xPosCmnd, out int xPos) && int.TryParse(yPosCmnd, out int yPos))
            {
                ThrowToPos(xPos, yPos);
            }
        }
    }

    private void Update()
    {
        if(updateTime < Time.time && isAI)
        {
            currentState.UpdateState(this);
            updateTime = Time.time + Random.Range(updateRate.x, updateRate.y);
        }

        if(aiTimeOutTime < Time.time && isAI == false)
        {
            isAI = true;
        }

        if(timeOutTime < Time.time)
        {
            manager.RemovePenguin(data.Name);
            RectTransform.DOKill();
            Destroy(gameObject);
        }
    }

    public void ChangePenguinState(PenguinState newState)
    {
        PenguinBaseState targetState = null;
        if(newState == PenguinState.Idle)
        {
            targetState = IdleState;
        }
        else if (newState == PenguinState.Walking)
        {
            targetState = WalkingState;
        }
        else if (newState == PenguinState.Sitting)
        {
            RectTransform.DOKill();
            targetState = SittingState;
        }
        else if (newState == PenguinState.Waving)
        {
            RectTransform.DOKill();
            LookAtDir(Vector2.down);
            targetState = WavingState;
        }
        else if (newState == PenguinState.Throwing)
        {
            RectTransform.DOKill();
            targetState = ThrowingState;
        }
        else if (newState == PenguinState.Dancing)
        {
            RectTransform.DOKill();
            LookAtDir(Vector2.down);
            targetState = DancingState;
        }
        if(targetState != null && targetState != currentState)
        {
            currentState?.ExitState(this);
        }
        currentState = targetState;

        penguinState = newState;
        currentState.EnterState(this);
        EventChangeState?.Invoke(penguinState);
    }

    public void ChangePenguinStateByAi(PenguinState newState)
    {
        if (newState == PenguinState.Idle)
        {
        }
        else if (newState == PenguinState.Walking)
        {
            MoveToRandomPos();
        }
        else if (newState == PenguinState.Sitting)
        {
            SitToDir(new Vector2(horizontalDir, verticalDir));
        }
        else if (newState == PenguinState.Waving)
        {
            ChangePenguinState(PenguinState.Waving);
        }
        else if (newState == PenguinState.Throwing)
        {
            ThrowAtRandom();
        }
        else if (newState == PenguinState.Dancing)
        {
            ChangePenguinState(PenguinState.Dancing);
        }
    }

    private void LookAtPoint(Vector2 point, bool allDirs = true)
    {
        Vector2 dir = (point - RectTransform.anchoredPosition).normalized;

        LookAtDir(dir, allDirs);
    }

    private void LookAtDir(Vector2 dir, bool allDirs = true)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float snappedAngle;
        int roundedX, roundedY;

        if (allDirs)
        {
            // Snap to 8 directions
            snappedAngle = Mathf.Round(angle / 45f) * 45f;
            roundedX = Mathf.RoundToInt(Mathf.Cos(snappedAngle * Mathf.Deg2Rad));
            roundedY = Mathf.RoundToInt(Mathf.Sin(snappedAngle * Mathf.Deg2Rad));
        }
        else
        {
            // Snap to 4 diagonal directions
            float[] snapAngles = { 45f, 135f, -135f, -45f };
            float minAngleDifference = Mathf.Abs(angle - snapAngles[0]);
            snappedAngle = snapAngles[0];

            foreach (float snapAngle in snapAngles)
            {
                float angleDifference = Mathf.Abs(angle - snapAngle);
                if (angleDifference < minAngleDifference)
                {
                    snappedAngle = snapAngle;
                    minAngleDifference = angleDifference;
                }
            }

            roundedX = Mathf.RoundToInt(Mathf.Cos(snappedAngle * Mathf.Deg2Rad));
            roundedY = Mathf.RoundToInt(Mathf.Sin(snappedAngle * Mathf.Deg2Rad));
        }

        horizontalDir = roundedX;
        verticalDir = roundedY;

        EventLookAt?.Invoke(horizontalDir, verticalDir);
    }

    public void MoveToPos(Vector2 pos)
    {
        ChangePenguinState(PenguinState.Walking);

        float distance = Vector2.Distance(pos, RectTransform.anchoredPosition);

        LookAtPoint(pos);

        RectTransform.DOKill();
        RectTransform.DOAnchorPos(pos, distance / walkingSpeed).SetEase(Ease.Linear)
        .OnUpdate(() =>
        {
            if(PolyCheck(RectTransform.anchoredPosition))
            {
                lastPos = RectTransform.anchoredPosition;
            }
            else
            {
                RectTransform.DOKill();
                RectTransform.anchoredPosition = lastPos;
                ChangePenguinState(PenguinState.Idle);
            }
        })
        .OnComplete(() =>
        {
            ChangePenguinState(PenguinState.Idle);
        });
    }

    public void MoveToRandomPos()
    {
        Vector2 randomPos = GetRandomScreenPos();
        MoveToPos(randomPos);
    }

    private void SitToDir(Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        LookAtDir(dir);
        ChangePenguinState(PenguinState.Sitting);
    }

    #region Throw
    private void ThrowToPos(Vector2 pos)
    {
        ChangePenguinState(PenguinState.Throwing);

        LookAtPoint(pos, false);

        throwTarget = pos;
    }

    public void ThrowSnowballAnimEvent()
    {
        Snowball newSnowball = SnowballManager.Instance.GetSnowball();

        RectTransform throwPivot = verticalDir > 0 ? throwPivotUp : throwPivotDown;

        Vector2 pivotPos = throwPivot.anchoredPosition;
        pivotPos.x = pivotPos.x * -horizontalDir;


        Vector2 initPos = pivotPos + RectTransform.anchoredPosition;
        newSnowball.GetComponent<RectTransform>().anchoredPosition = initPos;
        newSnowball.Shoot(throwTarget);
    }
    private void ThrowToPos(int x, int y)
    {
        x = Mathf.Clamp(x, 0, 1920);
        y = Mathf.Clamp(y, 0, 1080);
        Vector2 canvasPos = new Vector2(x - 960, y - 540);
        ThrowToPos(canvasPos);
    }
    private void ThrowAtRandom()
    {
        float rndm = Random.Range(0, 1.0f);
        if (rndm > 0.1f)
        {
            ThrowToRandomPos();
        }
        else
        {
            ThrowToRandomPenguin();
        }
    }
    private void ThrowToRandomPos()
    {
        ThrowToPos(Random.Range(0, 1920), Random.Range(0, 1080));
    }
    private void ThrowToPenguin(string penguinName)
    {
        Penguin p = manager.GetPenguin(penguinName);
        if(p != null)
        {
            ThrowToPos(p.RectTransform.anchoredPosition);
        }
    }
    private void ThrowToRandomPenguin()
    {
        Penguin p = manager.GetRandomPenguin();
        if (p != null && p != this)
        {
            ThrowToPos(p.RectTransform.anchoredPosition);
        }
        else
        {
            ThrowToRandomPos();
        }
    }
    #endregion
    public Vector2 GetRandomScreenPos()
    {
        float minX = float.PositiveInfinity;
        float maxX = float.NegativeInfinity;
        float minY = float.PositiveInfinity;
        float maxY = float.NegativeInfinity;

        // Determine the bounding box of the polygon
        Vector2[] collisionPoints = AreaManager.Instance.Path;
        foreach (Vector2 vertex in collisionPoints)
        {
            minX = Mathf.Min(minX, vertex.x);
            maxX = Mathf.Max(maxX, vertex.x);
            minY = Mathf.Min(minY, vertex.y);
            maxY = Mathf.Max(maxY, vertex.y);
        }

        // Generate random coordinates within the bounding box
        float randomX;
        float randomY;
        Vector2 randomPoint;
        bool isInside;

        do
        {
            randomX = Random.Range(minX, maxX);
            randomY = Random.Range(minY, maxY);
            randomPoint = new Vector2(randomX, randomY);

            // Check if the generated point is inside the polygon
            isInside = PolyCheck(randomPoint);
        } while (!isInside);

        return randomPoint;
    }

    public Vector2 GetDirWithCommand(string command)
    {
        int dirX = 0;
        int dirY = 0;

        if (command == "down")
        {
            dirX = 0;
            dirY = -1;
        }
        else if (command == "down-left" || command == "left-down")
        {
            dirX = -1;
            dirY = -1;
        }
        else if (command == "left")
        {
            dirX = -1;
            dirY = 0;
        }
        else if (command == "up-left" || command == "left-up")
        {
            dirX = -1;
            dirY = 1;
        }
        else if (command == "up")
        {
            dirX = 0;
            dirY = 1;
        }
        else if (command == "up-right" || command == "right-up")
        {
            dirX = 1;
            dirY = 1;
        }
        else if (command == "right")
        {
            dirX = 1;
            dirY = 0;
        }
        else if (command == "down-right" || command == "down-up")
        {
            dirX = 1;
            dirY = -1;
        }

        return new Vector2 (dirX, dirY);
    }

    private bool PolyCheck(Vector2 v)
    {
        Vector2[] collisionPoints = AreaManager.Instance.Path;
        int j = collisionPoints.Length - 1;
        bool c = false;
        for (int i = 0; i < collisionPoints.Length; j = i++) c ^= collisionPoints[i].y > v.y ^ collisionPoints[j].y > v.y && v.x < (collisionPoints[j].x - collisionPoints[i].x) * (v.y - collisionPoints[i].y) / (collisionPoints[j].y - collisionPoints[i].y) + collisionPoints[i].x;
        return c;
    }
}
