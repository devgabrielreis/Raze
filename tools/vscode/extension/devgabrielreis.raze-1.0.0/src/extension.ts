import * as vscode from 'vscode';

const TERMINAL_NAME = 'Raze';

function runFile(): void {
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
  terminal.sendText(`${executable} "${filePath}"`);
}

export function activate(context: vscode.ExtensionContext): void {
  let disposable = vscode.commands.registerCommand('raze.runFile', runFile);

  context.subscriptions.push(disposable);
}

export function deactivate(): void {}