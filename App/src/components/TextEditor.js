import React from 'react';
import { Editor } from 'react-draft-wysiwyg';
import '../../node_modules/react-draft-wysiwyg/dist/react-draft-wysiwyg.css';
import PropTypes from 'prop-types';

function TextEditor({ updateTextDescription, editorState }) {
  return (
    <div>
      <Editor
        wrapperClassName="demo-wrapper"
        editorClassName="demo-editor"
        editorState={editorState}
        onEditorStateChange={updateTextDescription}
        toolbar={{
          options: [
            'inline',
            'blockType',
            'fontSize',
            'fontFamily',
            'list',
            'textAlign',
            'colorPicker',
            'link',
          ],
          inline: {
            inDropdown: false,
            className: undefined,
            component: undefined,
            dropdownClassName: undefined,
            options: ['bold', 'italic', 'underline'],
          },
          blockType: {
            inDropdown: true,
            options: ['Normal', 'H2', 'H3'],
          },
          fontSize: {
            options: [14, 16],
          },
          fontFamily: {
            options: ['Open Sans'],
          },
          list: {
            inDropdown: false,
            className: undefined,
            component: undefined,
            dropdownClassName: undefined,
            options: ['unordered', 'ordered'],
          },
          textAlign: {
            inDropdown: false,
            className: undefined,
            component: undefined,
            dropdownClassName: undefined,
            options: ['left', 'center', 'right', 'justify'],
          },
          colorPicker: {
            colors: ['rgb(0, 53, 43)'],
          },
          link: {
            inDropdown: false,
            className: undefined,
            component: undefined,
            popupClassName: undefined,
            dropdownClassName: undefined,
            showOpenOptionOnHover: true,
            defaultTargetOption: '_self',
            options: ['link', 'unlink'],
            linkCallback: undefined,
          },
        }}
      />
    </div>
  );
}
export default TextEditor;
TextEditor.propTypes = {
  updateTextDescription: PropTypes.func,
  editorState: PropTypes.any,
};