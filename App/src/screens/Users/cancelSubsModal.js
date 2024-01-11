import React from 'react';
import CloseIcon from '@mui/icons-material/Close';
import Modal from 'react-bootstrap/Modal';

import PropTypes from "prop-types";

export default function CancelSubsModal(props) {
  return (
    <Modal
      {...props}
      aria-labelledby="contained-modal-title-vcenter"
      centered
      className="applyJobModal cancelSubModal"
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
          Keep Unlocking Value!
        </Modal.Title>
        <p style={{marginTop: "30px"}}><b>🚀 You're about to cancel your active membership plan. Are you sure you want to miss out on these incredible benefits?</b></p>
        <p>👔 <b>All NED, Advisory & Consultancy Roles in One Place:</b> Stay ahead in your career by accessing exclusive opportunities from top companies and organizations. Your next big role could be just a click away!</p>
        <p>📚 <b>Full Access to Learning and Resource Center:</b> Sharpen your skills, acquire new knowledge, and access a treasure trove of resources that can help you excel in your field.</p>
        <p>🤝 <b>Networking Opportunities:</b> Stay connected with a thriving community of professionals, share experiences, and network with peers and industry leaders.</p>
        <p>📊 <b>Data-Driven Insights:</b> Exclusive access to valuable industry insights and data that can help you make informed decisions and stay ahead of the curve.</p>
        <p>💼 <b>Professional Success:</b> By keeping your membership, you're investing in your professional success. Your career goals are within reach.</p>
        <div style={{display:"flex"}}>
            <button 
                onClick={props.onHide}
                className='saveButton jobModalButtonUpgrade'>
            Stay Subscribed
            </button>
            <button 
            onClick={props.handleCancelSubs}
            className='saveButton jobModalButtonLater'>
                Cancel Membership
            </button>
        </div>
      </Modal.Body>
    </Modal>
  );
}

CancelSubsModal.propTypes = {
  onHide: PropTypes.func,
  handleCancelSubs: PropTypes.func,
  }
