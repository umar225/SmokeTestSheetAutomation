import React from 'react';
import CloseIcon from '@mui/icons-material/Close';
import Modal from 'react-bootstrap/Modal';
import TextField from '@mui/material/TextField';

import PropTypes from "prop-types";



export default function ApplyJobModal(props) {
  return (
    <Modal
      {...props}
      size="md"
      aria-labelledby="contained-modal-title-vcenter"
      centered
      className="applyJobModal"
    >
      <Modal.Body className="applyJobModalBody">
        <CloseIcon
          onClick={props.onHide}
          className="closeButton"
          sx={{ fontSize: 20 }}
        />
        <Modal.Title
          id="contained-modal-title-vcenter"
          className="jobBoardHeading"
        >
          Apply now
        </Modal.Title>
          <TextField
            type="text"
            id="name"
            variant='filled'
            className="applyJobField"
            label="Full name"
            inputProps={{ maxLength: 50 }}
            value={props.fullName}
            onChange={(e) => {
              props.setFullName(e.target.value);
              props.setJobApplyError({ status: false, value: '' });
            }}
          />

          <TextField
            type="email"
            id="email"
            variant="filled"
            className="applyJobField"
            label="Email"
            value={props.email}
            onChange={(e) => {
              props.setEmail(e.target.value);
              props.setJobApplyError({ status: false, value: '' });
            }}
            onBlur={(e) => props.validateEmail(e.target.value)}
          />
          {!props.isValidEmail && props.email !== '' && (
            <p className="validationError">Please enter valid email.</p>
          )}
          <TextField
            type="text"
            id="valueToAdd"
            multiline
            minRows={2}
            maxRows={3}
            variant='filled'
            inputProps={{ maxLength: 200 }}
            label="How do you think you can add value?"
            value={props.valueToAdd}
            onChange={(e) => {
              props.setValueToAdd(e.target.value);
              props.setJobApplyError({ status: false, value: '' });
            }}
            className="applyJobField"
          />
          <div className="uploadCV">
            <div>
            <p className="applyJobUploadText">Upload CV</p>
            <p className="applyJobSmallText">Max. file size: 5 MB.</p>
            </div>
            <div>
          <button
            onClick={props.handleClick}
            className="browseButton"
          >
            {props.cv.name ? props.cv.name : 'Browse files'}
          </button>
          {props.cv.name && (
            <CloseIcon
              onClick={() => {
                props.hiddenFileInput.current.value = null;
                props.setCv('');
                props.setIsError(true);
                props.setJobApplyError({ status: false, value: '' });
              }}
              sx={{ fontSize: 10 }}
              className="fileDeleteIcon"
            />
          )}
          {!props.isValidFile.status && props.cv !== '' && (
            <p className="validationError">{props.isValidFile.value}</p>
          )}
          <input
            type="file"
            name="myCV"
            style={{ display: 'none' }}
            ref={props.hiddenFileInput}
            onChange={props.isUnSelect ? null : props.handleChange}
            accept=".pdf, .doc, .docx"
          />
        </div>
          </div>
        
        <button
          onClick={props.handleSendApplication}
          className=" applyButtonStyle applyText sendJobButton"
          disabled={
            props.isError || !props.isValidEmail || !props.isValidFile.status
          }
        >
          Send application
        </button>
        {props.jobApplyError.status && (
          <p className="validationError">{props.jobApplyError.value}</p>
        )}
      </Modal.Body>
    </Modal>
  );
}

ApplyJobModal.propTypes = {
  onHide: PropTypes.func,
  setFullName: PropTypes.func,
  setJobApplyError: PropTypes.func,
  handleChange: PropTypes.func,
  hiddenFileInput: PropTypes.object,
  setCv: PropTypes.func,
  isError: PropTypes.bool,
  setIsError: PropTypes.func,
  fullName: PropTypes.string,
  email: PropTypes.string,
  setEmail: PropTypes.func,
  validateEmail: PropTypes.func,
  setValueToAdd: PropTypes.func,
  isValidEmail: PropTypes.bool,
  handleClick: PropTypes.func,
  valueToAdd: PropTypes.string,
  isUnSelect: PropTypes.bool,
  handleSendApplication: PropTypes.func,
  cv: PropTypes.shape({
    name: PropTypes.string,
  
  }),
  isValidFile: PropTypes.shape({
    status: PropTypes.bool,
    value: PropTypes.string
  }),
  jobApplyError: PropTypes.shape({
    status: PropTypes.bool,
    value: PropTypes.string
  })
  
  }
