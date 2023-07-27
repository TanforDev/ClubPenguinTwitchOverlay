# Club Penguin Twitch Overlay

![Club Penguin Overlay](link_to_screenshot_or_gif_here)

## Description

Club Penguin Twitch Overlay is a fun and interactive overlay for Twitch streams that brings the spirit of Club Penguin to your viewers' screens. Penguins waddle around a transparent overlay, with each penguin representing a user in the Twitch chat. Viewers can control the penguins through chat commands, and all messages appear in a dialogue box above the corresponding penguin.

## Features

- Penguins waddle around the screen, representing Twitch chat users.
- Interact with the penguins using chat commands.
- Display Twitch chat messages in a dialogue box above each penguin.

## Installation

1. Download the latest release of the Club Penguin Twitch Overlay from the [Releases](link_to_releases_page) page.
2. Unzip the downloaded .zip file to your computer.

## Requirements

- Software for recording or streaming Twitch broadcasts (e.g., OBS, StreamLabs).

## Configuration

Before running the .exe file, customize the overlay's behavior by editing the `config.json` file located at `ClubPenguinTwitchOverlay\ClubPenguinTwitchOverlay_Data\StreamingAssets`.

- `user`: Your Twitch username for the overlay to connect to the Twitch chat.
- `oAuth`: OAuth token for your Twitch account for chat authentication. To connect to the Club Penguin Twitch Overlay, you will need to generate an OAuth token for your Twitch account. You can get this token here: [Generate Twitch OAuth Token](https://twitchapps.com/tmi/)
- `channel`: Name of your Twitch channel where the overlay will function.
- `maxPenguinCount`: Maximum number of penguins displayed on the overlay.
- `joinCommandToJoin`: Set to true if viewers need to use chat command `!join` to spawn a penguin.
- `penguinTimeout`: Time in seconds for a penguin to disappear if the corresponding viewer becomes inactive.
- `showBadges`: Set to true to display Twitch chat badges next to penguins.
- `maxSnowballCount`: Maximum number of snowballs on screen.
- `targetFrameRate`: Desired frame rate (in FPS) of the overlay's animation.
- `screenHeight`: Height of the overlay screen or canvas (width determined by 16:9 aspect ratio).

## Usage

1. Open your preferred broadcasting software (OBS, StreamLabs, etc.).
2. Launch `ClubPenguinTwitchOverlay.exe` (it will run in the background).
3. Add a new "Game Capture" to your overlay scene.
4. Set the Game Capture source to "ClubPenguinTwitchOverlay.exe".
5. Enable "Allow Transparency" in the Game Capture source settings (IMPORTANT).

## Chat Commands
- `!join`: Make your penguin spawn on screen.
- `!move`: Make your penguin waddle to a random point on the screen.
- `!sit`: Make your penguin sit in place.
- `!dance`: Make your penguin dance with joy.
- `!wave`: Make your penguin wave to the crowd.
- `!throw`: Make your penguin throw a snowball to a random point on the screen.
- `!emote [emote_name]`: Trigger a specific emote animation for your penguin.
- `!afk`: Make your penguin be controlled by CPU.

### Advanced Commands

- `!move [direction]`: Make your penguin move to a specific direction.
- `!move [direction] [amount]`: Make your penguin move by a specified amount (between 0.1-10).
- `!move [xPos] [yPos]`: Make your penguin move to a specific position on the screen.
- `!sit [direction]`: Make your penguin sit facing a specific direction.
- `!throw [xPos] [yPos]`: Make your penguin throw a snowball to a specific position on the screen.
- `!throw [penguin name]`: Make your penguin throw a snowball to another penguin's position.

Example of directions: up, down, left, right, up-left, right-down, etc...

## Screenshots

![Overlay in Action](link_to_screenshot_or_gif_here)

## Contributing

We welcome contributions to improve the Club Penguin Twitch Overlay project. If you'd like to contribute, please follow these guidelines:

1. Fork the repository and create a new branch.
2. Make your changes and test thoroughly.
3. Submit a pull request, describing the changes and improvements made.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Credits

- All code was written by [@TanforDev](https://github.com/TanforDev).
- Penguin sprites were made by [@IBarkSometimes](https://twitter.com/IBarkSometimes).
- Twitch Connectivity by [@minionsart](https://twitter.com/minionsart/status/1412400308156837895).
