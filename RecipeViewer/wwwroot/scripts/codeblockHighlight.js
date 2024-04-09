export function applySyntaxHighlightingWithJQuery() {
    $(document).ready(function () {
        $('pre code').each(function (i, block) {
            hljs.highlightElement(block);
        });
    });

}
