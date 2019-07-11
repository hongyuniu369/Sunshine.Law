﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Uploader.aspx.cs" Inherits="Utility_ckfinder_plugins_flashupload_Uploader" %>

<html lang="en" >
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
<style>
body {
	margin: 0px;
	overflow:hidden;
	padding: 5px 0;
}
#cke_flashupload {
	display:block;
	z-index:100;
}
</style>
</head>
<body scroll="no">
    <form id="form1" runat="server">
<div id="cke_flashupload" tabindex="0"></div>
<script language="JavaScript" type="text/javascript">
    /**
    * Protection against scrolling browser window.
    * @param event
    */
    function wheel(event) {
        var e = event || window.event;
        e.cancelBubble = true;
        if (e.stopPropagation) e.stopPropagation();
        return false;
    }
    if (window.addEventListener) {
        window.addEventListener('DOMMouseScroll', wheel, false);
    }
    else {
        window.attachEvent("onscroll", wheel);
    }
    window.onmousewheel = document.onmousewheel = wheel;

    var lang = window.parent.api.lang;
    var api = window.parent.api;
    var params = {
        width: "100%",
        height: "100%",
        id: "Uploader",
        quality: "high",
        bgcolor: "#FFFFFF",
        name: "Uploader",
        wmode: "transparent",
        allowScriptAccess: "sameDomain"
    };
    // flashvars must be encoded due to problems in IE
    // http://code.google.com/p/swfobject/issues/detail?id=66
    var flashvars = {
        layoutDirection: lang.dir,
        browseButtonLabel: encodeURIComponent(lang.UploadAddFiles),
        totalSizeLabel: encodeURIComponent(lang.UploadTotalSize),
        totalFilesLabel: encodeURIComponent(lang.UploadTotalFiles),
        clearButtonLabel: encodeURIComponent(lang.UploadClearFiles),
        cancelButtonLabel: encodeURIComponent(lang.CancelBtn),
        closeButtonLabel: encodeURIComponent(lang.CloseBtn),
        uploadButtonLabel: encodeURIComponent(lang.UploadSend),
        cancelUploadButtonLabel: encodeURIComponent(lang.UploadCancel),
        removeButtonLabel: encodeURIComponent(lang.UploadRemove),
        removeTip: encodeURIComponent(lang.UploadRemoveTip),
        uploadedLabel: encodeURIComponent(lang.UploadUploaded),
        fileTooLargeMsg: encodeURIComponent(lang.Errors[203]),
        processingMsg: encodeURIComponent(lang.UploadProcessing),
        uploadMaxSize: window.parent.api.config.uploadMaxSize,
        uploadCheckImages: window.parent.api.config.uploadCheckImages ? 1 : 0,
        isIE: window.parent.CKFinder.env.ie ? 1 : 0
    };
    var attributes = {};
    var swfobject = window.parent.create_swfobject(window, document);
    swfobject.embedSWF("flash/Uploader.swf", "cke_flashupload", "100%", "100%", "10.2.0", "", flashvars, params, attributes);
    /**
    * Functions called by Flash uploader.
    */
    function getAllowedExtensions() {
        var api = window.parent.api;
        return api.getSelectedFolder().getResourceType().allowedExtensions;
    }
    function getFilterDescription() {
        var api = window.parent.api;
        return api.getSelectedFolder().type;
    }
    function getUploadUrl() {
        var api = window.parent.api;
        return api.getSelectedFolder().getUploadUrl() + "&response_type=txt&ASPSESSID=<%=Session.SessionID %>";
    }
    function getMaxSize() {
        var api = window.parent.api;
        return parseInt(api.getSelectedFolder().getResourceType().maxSize, 10);
    }
    // Name of cookies used as session identifiers and the name to be used in the URL.
    // An unfortunate requirement of flash uploader due to infamous cookie bug in Flash.
    function getSessionIdentifiers() {
        return window.parent.sessionIdentifiers;
    }
    function getCookies() {
        // Loaded via LoadCookies
        if (window.parent.flash_cookies)
            return window.parent.flash_cookies;

        // document.cookie does not store HttpOnly cookies
        var cookie, cookies = {};
        if (window.parent.document.cookie.length) {
            var cookieArray = window.parent.document.cookie.split(';'),
			name, eqIndex;
            for (var i = cookieArray.length - 1; i > 0; i--) {
                cookie = cookieArray[i];
                cookie = cookie.replace(/^( )+/g, "");
                eqIndex = cookie.indexOf("=");
                if (eqIndex > 0) {
                    name = cookie.substring(0, eqIndex);
                    if (name.indexOf("CKFinder_") !== 0)
                        cookies[cookie.substring(0, eqIndex)] = cookie.substring(eqIndex + 1);
                }
            }
        }
        return cookies;
    }
    function focusDocument() {
        setTimeout(function () { document.focus; }, 200);
    }
    function showFiles(selectedFile) {
        var api = window.parent.api;
        api.getSelectedFolder().showFiles(selectedFile);
    }
</script>
    </form>
</body>
</html>
