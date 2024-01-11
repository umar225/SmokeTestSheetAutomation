import React from 'react';
import Modal from 'react-bootstrap/Modal';
import { useNavigate } from 'react-router-dom';
import PropTypes from 'prop-types'
import Amplitude from '../../utils/Amplitude';


export default function JobsBoardModal(props) {
    const navigate = useNavigate();
  return (
    <Modal
        {...props}
        aria-labelledby="contained-modal-title-vcenter"
        centered
        className="applyJobModal"
        dialogClassName="jobBoardModal"
    >
    <Modal.Body>
      <Modal.Title
      id="contained-modal-title-vcenter"
      className="jobBoardModalHeading"
    >
      Upgrade now to apply for exclusive jobs!
    </Modal.Title>
      <p className="jobBoardModalText">Apply for our fully exclusive Board appointments in the private sector, from tech companies to extreme sports brands.
      Make the most of our resources and quick reads covering topics from Board influence to Coaching and mentoring.</p>
      <div className="jobModalButtons">
        <button 
          onClick={()=>{
            Amplitude("Upgrade on Jobs");
            navigate("/pricing");
          }}
          className='saveButton jobModalButtonUpgrade'>
            Upgrade Now
        </button>
        <button 
          onClick={props.onHide}
          className='saveButton jobModalButtonLater'>
            Maybe Later
        </button>
      </div>
    </Modal.Body>
  </Modal>
  );
}

JobsBoardModal.propTypes = {
  onHide: PropTypes.func
}