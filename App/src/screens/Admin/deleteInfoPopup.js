import React from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import { Container, Row, Col } from 'react-bootstrap';
import PropTypes from 'prop-types';

export default function DeleteInfoPopup({ show, handleClose, deletePress }) {
  return (
    <div>
      <Modal size="md" show={show} onHide={handleClose}>
        <Modal.Body>
          <Container>
            <div className="courseDetail crsModalWidth">
              <Row>
                <Col sm={12}>
                  <p>Are you sure you want to delete?</p>
                </Col>
              </Row>
            </div>
          </Container>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Cancel
          </Button>
          <Button id="confirmDelete" variant="primary" onClick={deletePress}>
            Confirm
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
}
DeleteInfoPopup.propTypes = {
  show: PropTypes.bool,
  handleClose: PropTypes.func,
  deletePress: PropTypes.func
};