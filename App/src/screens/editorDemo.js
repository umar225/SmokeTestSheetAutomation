import React, { useState } from 'react';
import { Editor } from 'react-draft-wysiwyg';
import { EditorState, convertToRaw } from 'draft-js';
import '../../node_modules/react-draft-wysiwyg/dist/react-draft-wysiwyg.css';
import draftToHtml from 'draftjs-to-html';
import parse from 'html-react-parser';
import { Header } from '../components/header/index';
import Footer from '../components/footer/footer';

export default function EditorDemo() {
  const [editorState, setEditorState] = useState(() =>
    EditorState.createEmpty()
  );
  const [finalHtml, setFinalHtml] = useState('');
  const updateTextDescription = async (state) => {
    await setEditorState(state);
  };
  const onFinalDataClick = () => {
    const value = draftToHtml(convertToRaw(editorState.getCurrentContent()));
    setFinalHtml(value);
  };

  return (
    <div>
      <Header />
      <div className="bodyWrapper">
        <div className="mainWrapper gapMargin">
          <div className="whiteBackground">
            <div className="editorTestMain">
              <div>
                <Editor
                  wrapperClassName="demo-wrapper"
                  editorClassName="demo-editor"
                  // onFocus={(event) => {}}
                  // onBlur={(event, editorState) => {}}
                  // onTab={(event) => {}}
                  editorState={editorState}
                  onEditorStateChange={updateTextDescription}
                />
                <button onClick={() => onFinalDataClick()}>Get value</button>
              </div>
              <div>
                <h3>Final Result</h3>
                {parse(finalHtml)}
              </div>
            </div>
          </div>
        </div>
      </div>
      <div className="footerWrapper">
        <Footer />
      </div>
    </div>
  );
}
