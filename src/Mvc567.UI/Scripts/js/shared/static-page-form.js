try {
    let htmlEditor = CodeMirror.fromTextArea(document.getElementById("page-html-editor"), {
        lineNumbers: true,
        mode: "text/html",
        showCursorWhenSelecting: true,
        theme: "eclipse",
        tabSize: 2
    });
    htmlEditor.setSize(null, 300);

    let scriptsEditor = CodeMirror.fromTextArea(document.getElementById("page-scripts-editor"), {
        lineNumbers: true,
        mode: "javascript",
        autoCloseBrackets: true,
        matchBrackets: true,
        showCursorWhenSelecting: true,
        theme: "eclipse",
        tabSize: 2
    });
    scriptsEditor.setSize(null, 300);

    let scriptsEditorMin = CodeMirror.fromTextArea(document.getElementById("page-scripts-editor-min"), {
        lineNumbers: true,
        mode: "javascript",
        autoCloseBrackets: true,
        matchBrackets: true,
        showCursorWhenSelecting: true,
        theme: "eclipse",
        tabSize: 2,
        readOnly: true
    });
    scriptsEditorMin.setSize(null, 50);

    let styleEditor = CodeMirror.fromTextArea(document.getElementById("page-style-editor"), {
        lineNumbers: true,
        mode: "text/css",
        autoCloseBrackets: true,
        matchBrackets: true,
        showCursorWhenSelecting: true,
        theme: "eclipse",
        tabSize: 2
    });
    styleEditor.setSize(null, 300);

    $('#obfuscate-page-scripts-btn').on('click', function () {
        let scriptEditorContent = scriptsEditor.getValue();
        let obfuscatedResult = JavaScriptObfuscator.obfuscate(scriptEditorContent);
        scriptsEditorMin.setValue(obfuscatedResult.obfuscatedCode);
    });

} catch (e) {
    //
}