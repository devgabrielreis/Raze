import * as vscode from 'vscode';

const TERMINAL_NAME = 'Raze';

async function sleep(ms: number) {
  return new Promise(res => setTimeout(res, ms));
}

async function runFile(): Promise<void> {
  const editor = vscode.window.activeTextEditor;
  if (!editor) {
    vscode.window.showErrorMessage('No files open.');
    return;
  }

  const filePath = editor.document.fileName;
  const config = vscode.workspace.getConfiguration('raze');
  const executable = config.get('executableName') || 'raze';

  let terminal = vscode.window.terminals.find(t => t.name == TERMINAL_NAME);

  if (!terminal) {
    terminal = vscode.window.createTerminal(TERMINAL_NAME);
  }

  terminal.show();

  terminal.sendText('\x03'); // Ctrl+C
  await sleep(500);

  terminal.sendText(`${executable} "${filePath}"`);
}

export function activate(context: vscode.ExtensionContext): void {
  let disposable = vscode.commands.registerCommand('raze.runFile', () => {
    runFile().catch(err => {
      vscode.window.showErrorMessage(`Error: ${err.message}`);
    });
  });

  context.subscriptions.push(disposable);
}

export function deactivate(): void {}