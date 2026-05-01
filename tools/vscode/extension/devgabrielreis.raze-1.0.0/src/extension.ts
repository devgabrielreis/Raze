import * as vscode from 'vscode';

function runFile(): void {
  const editor = vscode.window.activeTextEditor;
  if (!editor) {
    vscode.window.showErrorMessage('No files open.');
    return;
  }

  const filePath = editor.document.fileName;
  const config = vscode.workspace.getConfiguration('raze');
  const executable = config.get('executableName') || 'raze';

  const terminal = vscode.window.createTerminal('Raze');
  terminal.show();
  terminal.sendText(`${executable} "${filePath}"`);
}

export function activate(context: vscode.ExtensionContext): void {
  let disposable = vscode.commands.registerCommand('raze.runFile', runFile);

  context.subscriptions.push(disposable);
}

export function deactivate(): void {}