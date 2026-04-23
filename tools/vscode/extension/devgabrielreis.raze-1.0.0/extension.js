const vscode = require('vscode');

function runFile() {
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

function activate(context) {
  let disposable = vscode.commands.registerCommand('raze.runFile', runFile);

  context.subscriptions.push(disposable);
}

function deactivate() {}

module.exports = { activate, deactivate };