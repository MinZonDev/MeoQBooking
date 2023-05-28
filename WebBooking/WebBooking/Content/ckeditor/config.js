/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.filebrowserBrowseUrl = "/Areas/Admin/assets/plugin/ckfinder/ckfinder.html";
    config.filebrowserImageUrl = "/Areas/Admin/assets/plugin/ckfinder/ckfinder.html?type=Images";
    config.filebrowserFlashUrl = "/Areas/Admin/assets/plugin/ckfinder/ckfinder.html?type=Flash";
    config.filebrowserUploadUrl = "/Areas/Admin/assets/plugin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files";
    config.filebrowserImageUploadUrl = "/Areas/Admin/assets/plugin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images";
    config.filebrowserFlashUploadUrl = "/Areas/Admin/assets/plugin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash";

    config.extraPlugins = 'youtube';
    config.youtube_responsive = true;
};
