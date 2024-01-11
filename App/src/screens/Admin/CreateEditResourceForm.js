import React from 'react';
import { Row, Col } from 'react-bootstrap';
import Loader from '../../components/Loader';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import { AiFillDelete } from 'react-icons/ai';
import TextEditor from '../../components/TextEditor';
import PropTypes from 'prop-types';

const FormHeading = ({location}) => {
  return (
    <h3>
      {location.state.id !== 'new' ? 'Update Resource' : 'Add Resource'}
    </h3>
  );
};

const ImageErrorText = ({ isError, image, logoSizeErr }) => {
  return (
    <>
      {isError && image === '' ? (
        <p className="errorTxt">Please select image.</p>
      ) : null}
      {logoSizeErr.status ? (
        <p className="errorTxt">{logoSizeErr.text}</p>
      ) : null}
    </>
  );
};


const FeaturedRadioButtons = ({isFeatured, setIsFeatured}) => {
  return (
    <div className="jobCatWrapper">
      <span className="externalCat">
        <input           
          type="radio"
          id="cat1"
          name="featured"
          value="Yes"
          checked={isFeatured === 'Yes'}
          onChange={(e) => setIsFeatured(e.target.value)}
        />
        <span>Yes</span>
      </span>
      <span className="externalCat">
        <input
          type="radio"
          id="cat2"
          name="featured"
          value="No"
          checked={isFeatured === 'No'}
          onChange={(e) => setIsFeatured(e.target.value)}
        />
        <span>No</span>
      </span>
    </div>
  );
};

const NotificationRadioButtons = ({sendNotification, setSendNotification}) => {
  return (
    <div className="jobCatWrapper">
      <span className="externalCat">
        <input           
          type="radio"
          id="cat1"
          name="notification"
          value="Yes"
          checked={sendNotification === "Yes"}
          onChange={(e) => setSendNotification(e.target.value)}
        />
        <span>Yes</span>
      </span>
      <span className="externalCat">
        <input
          type="radio"
          id="cat2"
          name="notification"
          value="No"
          checked={sendNotification === "No"}
          onChange={(e) => setSendNotification(e.target.value)}
        />
        <span>No</span>
      </span>
    </div>
  );
};

const ResourceTypeRadio = ({resourceType, setResourceType}) => {
  return (
    <div className="jobCatWrapper">
      <span className="externalCat">
        <input            
          type="radio"
          id="cat1"
          name="resourceType"
          value={parseInt(1, 10)}
          checked={resourceType === 1}
          onChange={() => setResourceType(1)}
        />
        <span>Image resource</span>
      </span>
      <span className="externalCat">
        <input
          type="radio"
          id="cat2"
          name="resourceType"
          value={parseInt(2, 10)}
          checked={resourceType === 2}
          onChange={() => setResourceType(2)}
        />
        <span>Video Resource</span>
      </span>
    </div>
  );
};

const CreateEditResourceForm = ({
  isLoading,
  resourceTitle,
  setResourceTitle,
  shortDes,
  setShortDes,
  image,
  setImage,
  selectImage,
  onImageChange,
  logoSizeErr,
  updateTextDescription,
  editorState,
  desError,
  readLength,
  setReadLength,
  onAddUpdateResource,
  isFeatured,
  setIsFeatured,
  resourceType,
  setResourceType,
  isError,
  location,
  onCancel,
  setShowDelete,
  youtubeVidId,
  setYoutubeVidId,
  sendNotification,
  setSendNotification
}) => {

  return (
    <div className="editorPageWidth">
      {isLoading && <Loader />}

      <Row>
        <Col sm={10}>
          <Form>
            <Row>
              <Col xs={6}>
                <FormHeading location={location} />
              </Col>
              <Col xs={6} className="editCourseBtnWrap">
                  {location?.state?.id !== 'new' && (
                    <Button
                      id="deleteBtn"
                      variant="outline-danger"
                      className="deleteBtn"
                      onClick={() => setShowDelete(true)}

                    >
                      Delete
                    </Button>
                  )}
                </Col>
            </Row>
            <Form.Group className="mb-3">
              <Form.Label>Resource title</Form.Label>
              <Form.Control
                type="text"
                id="resourceTitle"
                placeholder="Enter Resource Title"
                value={resourceTitle}
                required={true}
                onChange={(e) => {
                  setResourceTitle(e.target.value);
                }}
              />
              {isError && resourceTitle === '' && (
                <p className="errorTxt">Please enter resource title.</p>
              )}
            </Form.Group>

            <Form.Group className="mb-3">
              <Form.Label>Preview text</Form.Label>
              <Form.Control
                placeholder="Enter Preview Text"
                onChange={(e) => {
                  setShortDes(e.target.value);
                }}
                as="textarea"
                rows={3}
                id="previewText"
                value={shortDes}
                required={true}
              />
              <p className="helpingTxt">
                {'Maximum length is 200 characters. '}
                {shortDes.length}
              </p>
              {isError && shortDes === '' && (
                <p className="errorTxt">Please enter preview text.</p>
              )}
              {shortDes.length > 200 && (
                <p className="errorTxt">
                  Preview text has exceeded the maximum length.
                </p>
              )}
            </Form.Group>

            <Form.Group className="mb-3">
              <div className="resourceImgCont">
                <Form.Label>Image</Form.Label>
                {image !== '' && (
                  <span>
                    <AiFillDelete
                      onClick={() => setImage('')}
                      className="resourceDelete"
                    />
                  </span>
                )}
              </div>
              <div>
                {image !== '' ? (
                  <div className="resourceImg">
                    <img src={selectImage(image)} alt="resource-img" />
                  </div>
                ) : (
                  <div className="imgUploadWrapper">
                    <input
                      type="file"
                      name="myImage"
                      onChange={onImageChange}
                      accept="image/*"
                    />
                  </div>
                )}
              </div>
              <div className='resourceImgCaption'>
                For best results, upload an image that is 1950px by 450px or larger.
              </div>
              <ImageErrorText isError={isError} image={image} logoSizeErr={logoSizeErr}/>
            </Form.Group>
            <Form.Group className="mb-3 inputRoles">
              <Form.Label>Resource Type</Form.Label>
              <ResourceTypeRadio resourceType={resourceType} setResourceType={setResourceType}/>
            </Form.Group>
            {resourceType === 2 && <Form.Group className="mb-3">
              <Form.Label>Youtube Video Id</Form.Label>
              <Form.Control
                type="text"
                id="youtubeVideo"
                placeholder="Enter Youtube Video ID"
                value={youtubeVidId}
                required={true}
                onChange={(e) => {
                  setYoutubeVidId(e.target.value);
                }}
              />
              {isError && youtubeVidId === '' && (
                <p className="errorTxt">Please enter video Id.</p>
              )}
            </Form.Group>}
            {location.state.id === 'new' &&
            <Form.Group className="mb-3 inputRoles">
              <Form.Label>Send Notification to Users</Form.Label>
              <NotificationRadioButtons sendNotification={sendNotification} setSendNotification={setSendNotification}/>
            </Form.Group>}
            <Form.Group className="mb-3">
              <Form.Label>Description</Form.Label>
              <div className="ckEditorWrapper">
                <TextEditor
                  updateTextDescription={updateTextDescription}
                  editorState={editorState}
                />
              </div>
              {desError ? (
                <p className="errorTxt">Please enter resource description.</p>
              ) : null}
            </Form.Group>

            <Form.Group className="mb-3 inputRoles">
              <Form.Label>
                Estimated length of time to read this resource
              </Form.Label>
              <Form.Control
                type="number"
                id="length"
                placeholder="Enter length of article"
                value={readLength}
                required={true}
                onChange={(e) => {
                  setReadLength(e.target.value);
                }}
              />
              {isError && readLength === '' ? (
                <p className="errorTxt">
                  Please enter a number, which should be the estimated length in
                  minutes.
                </p>
              ) : null}
              {readLength !== '' && readLength % 1 !== 0 ? (
                <p className="errorTxt">
                  Please enter integer value the estimated length in minutes.
                </p>
              ) : null}
              {Number(readLength) > 1440 ? (
                <p className="errorTxt">
                  Estimated length cannot exceed from 120 minutes.
                </p>
              ) : null}
            </Form.Group>

            <Form.Group className="mb-3 inputRoles">
              <Form.Label>Featured</Form.Label>
              <FeaturedRadioButtons isFeatured={isFeatured} setIsFeatured={setIsFeatured}/>
            </Form.Group>

            <br />
            <Button
              id="submitCourse"
              variant="primary"
              type="submit"
              onClick={onAddUpdateResource}
            >
              Submit
            </Button>
            {location.state.id !== 'new' && (
              <Button
                variant="secondary"
                onClick={onCancel}
                className="marginLeft"
              >
                Cancel
              </Button>
            )}
          </Form>
        </Col>
      </Row>
    </div>
  );
};

export default CreateEditResourceForm;
FormHeading.propTypes = {
  location: PropTypes.any,
}
ImageErrorText.propTypes = {
  image: PropTypes.any,
  logoSizeErr: PropTypes.object,
  isError: PropTypes.bool,
}
FeaturedRadioButtons.propTypes = {
  isFeatured: PropTypes.string,
  setIsFeatured: PropTypes.func,
}
NotificationRadioButtons.propTypes = {
  sendNotification: PropTypes.string,
  setSendNotification: PropTypes.func,
}
ResourceTypeRadio.propTypes = {
  resourceType: PropTypes.number,
  setResourceType: PropTypes.func,
}
CreateEditResourceForm.propTypes = {
  isLoading: PropTypes.bool,
  resourceTitle: PropTypes.string,
  setResourceTitle: PropTypes.func,
  shortDes: PropTypes.string,
  setShortDes: PropTypes.func,
  image: PropTypes.any,
  setImage: PropTypes.func,
  selectImage: PropTypes.func,
  onImageChange: PropTypes.func,
  logoSizeErr: PropTypes.object,
  updateTextDescription: PropTypes.func,
  editorState: PropTypes.any,
  desError: PropTypes.bool,
  readLength: PropTypes.string,
  setReadLength: PropTypes.func,
  onAddUpdateResource: PropTypes.func,
  isFeatured: PropTypes.string,
  setIsFeatured: PropTypes.func,
  resourceType: PropTypes.number,
  setResourceType: PropTypes.func,
  isError: PropTypes.bool,
  location: PropTypes.any,
  onCancel: PropTypes.func,
  setShowDelete: PropTypes.func,
  youtubeVidId: PropTypes.string,
  setYoutubeVidId: PropTypes.func,
  sendNotification: PropTypes.string,
  setSendNotification: PropTypes.func,
};