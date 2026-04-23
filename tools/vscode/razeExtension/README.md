# raze

Syntax highlighting and script execution for the Raze language.

## Install the extension

* To start using this extension with Visual Studio Code copy it into the `<user home>/.vscode/extensions` folder and restart Code.

## Configuration
| Setting | Type | Default | Description |
|---|---|---|---|
| `raze.executableName` | `string` | `raze` | The executable used to run .raze files (e.g. `raze`, `/usr/local/bin/raze`). |

Settings can be changed in **File → Preferences → Settings** by searching for `Raze`, or defined per workspace in `.vscode/settings.json`:
```json
{
  "raze.executableName": "/usr/local/bin/raze"
}
```