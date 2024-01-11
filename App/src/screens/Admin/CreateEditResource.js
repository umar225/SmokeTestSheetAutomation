import React, { useState, useRef, useEffect } from 'react';
import { AdminHeader } from '../../components/header/index';
import Footer from '../../components/footer/footer';
import { Container } from 'react-bootstrap';
import { useNavigate, useLocation } from 'react-router-dom';
import { adminCourseActions } from '../../store/adminCourses/adminCourseActions';
import { useDispatch } from 'react-redux';
import {
  EditorState,
  convertFromRaw,
  convertToRaw,
  convertFromHTML,
  ContentState,
} from 'draft-js';
import draftToHtml from 'draftjs-to-html';
import { toast } from 'react-toastify';
import CreateEditResourceForm from './CreateEditResourceForm';
import DeleteInfoPopup from './deleteInfoPopup';

const CreateEditResource = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const location = useLocation();

  const [isLoading, setIsLoading] = useState(true);
  const [resourceTitle, setResourceTitle] = useState('');
  const [shortDes, setShortDes] = useState('');
  const [readLength, setReadLength] = useState('');
  const [editorState, setEditorState] = useState(() =>
    EditorState.createEmpty()
  );
  const [image, setImage] = useState('');
  const [mediaId, setMediaId] = useState('');
  const [isFeatured, setIsFeatured] = useState('No');
  const [youtubeVidId, setYoutubeVidId] = useState('');
  const [resourceType, setResourceType] = useState(1);
  const [sendNotification, setSendNotification] = useState('No');
  const [isError, setIsError] = useState(false);
  const [desError, setDesError] = useState(false);
  const [logoSizeErr, setLogoSizeErr] = useState({ status: false, text: '' });
  const [showDelete, setShowDelete] = useState(false);

  useEffect(() => {
    if (location.state.id !== 'new') {
      const resourceId = location.state.id;
      getResource(resourceId);
    } else {
      setIsLoading(false);
    }
  }, []);

  const getResource = (resourceId) => {
    dispatch(adminCourseActions.onGetResourceById(resourceId)).then((res) => {
      if (res?.success && res.data) {
        setResourceTitle(res.data.title);
        setShortDes(res.data.preview);
        setReadLength(res.data.length);
        setImage(res.data.media.length > 0 ? res.data.media[0].url : '');
        setMediaId(res.data.media.length > 0 ? res.data.media[0].id : '');
        setIsFeatured(res.data.isFeatured ? 'Yes' : 'No');
        setResourceType(res.data.resourceType);
        setYoutubeVidId(res.data.videoLink);
        const contentBlocks = convertFromHTML(res.data.description);
        const contentState = ContentState.createFromBlockArray(contentBlocks);
        const finalResult = convertToRaw(contentState);
        setEditorState(
          EditorState.createWithContent(convertFromRaw(finalResult))
        );
        setIsLoading(false);
      }
    });
  };

  const updateTextDescription = async (state) => {
    await setEditorState(state);
    convertToRaw(editorState.getCurrentContent());
    setDesError(false);
  };

  const selectImage = (value) => {
    if (location.state.id !== 'new') {
      return value;
    } else {
      return URL.createObjectURL(value);
    }
  };

  const updateMedia = (img) => {
    dispatch(
      adminCourseActions.onUpdateResourceMedia({
        id: location.state.id,
        media: img,
      })
    ).then((res) => {
      if (res?.success) {
        toast.success('Resource Media Updated successfully');
        setImage(res.data?.url);
        setIsLoading(false);
        deleteMedia(res.data);
      } else {
        toast.error(res?.message);
        setIsLoading(false);
      }
    });
  };

  const validateMedia = (img) => {
    return (
      img.type === 'image/png' ||
      img.type === 'image/jpeg' ||
      img.type === 'image/jpg'
    );
  };

  const onImageChange = (event) => {
    if (event.target.files?.[0]) {
      if (event.target.files[0].size <= 5242880) {
        setLogoSizeErr({ status: false, text: '' });
        let img = event.target.files[0];
        if (validateMedia(img)) {
          if (location.state.id !== 'new') {
            setIsLoading(true);
            updateMedia(img);
          } else {
            setImage(img);
          }
        } else {
          setLogoSizeErr({
            status: true,
            text: 'Please select png, jpg or jpeg file type.',
          });
        }
      } else {
        setLogoSizeErr({
          status: true,
          text: 'Maximum image size can be 5 MB.',
        });
      }
    }
  };

  const deleteMedia = (uploadedImg) => {
    if (location.state.id !== 'new') {
      dispatch(adminCourseActions.onDeleteResourceMedia(mediaId)).then(
        (res) => {
          if (res?.success) {
            setMediaId(uploadedImg.id);
          } else {
            toast.error(res?.message);
          }
        }
      );
    } else {
      setImage('');
    }
  };

  const updateResource = (resourcePayload) => {
    dispatch(adminCourseActions.onUpdateResource(resourcePayload)).then(
      (res) => {
        setIsLoading(false);
        if (res?.success) {
          navigate(-1);
          toast.success('Resource Updated successfully');
        } else {
          toast.error(res?.message);
        }
      }
    );
  };

  const addNewResource = (resourcePayload) => {
    dispatch(adminCourseActions.onAddResource(resourcePayload)).then((res) => {
      setIsLoading(false);
      if (res?.success) {
        navigate(-1);
        toast.success('Resource added successfully');
      } else {
        toast.error(res?.message);
      }
    });
  };

  const onAddUpdateResource = async (e) => {
    e.preventDefault();
    const value = draftToHtml(convertToRaw(editorState.getCurrentContent()));
    if (
      resourceTitle !== '' &&
      shortDes !== '' &&
      shortDes.length <= 200 &&
      image !== '' &&
      readLength !== '' &&
      value !== '' &&
      value.length > 8
    ) {
      setIsError(false);
      setIsLoading(true);
      const resourcePayload = {
        title: resourceTitle,
        preview: shortDes,
        description: value,
        length: Number(readLength),
        isFeatured: isFeatured === 'Yes',
        resourceType: resourceType,
        videoLink: youtubeVidId,
      };
      if (location.state.id !== 'new') {
        resourcePayload.id = location.state.id;
        updateResource(resourcePayload);
      } else {
        resourcePayload.media = image;
        resourcePayload.SendNotification = sendNotification === 'Yes';
        addNewResource(resourcePayload);
      }
    } else {
      if (value.length <= 8) {
        setDesError(true);
      } else {
        setDesError(false);
      }
      setIsError(true);
    }
  };

  const deleteJobById = () => {
    setIsLoading(true);
    dispatch(adminCourseActions.onDeleteResource(location.state.id)).then((data) => {
      setIsLoading(false);
      if (data?.success) {
        navigate(-1);
      } else {
        toast.error(data?.message);
      }
    });
  }

  const onCancel = () => {
    navigate(-1);
  };

  const scrollToView = useRef(null);
  return (
    <div>
      <AdminHeader />
      <div className="bodyWrapper">
        <div className="whiteBackground">
          <Container className="projectDetail" ref={scrollToView}>
            <CreateEditResourceForm
              isLoading={isLoading}
              resourceTitle={resourceTitle}
              setResourceTitle={setResourceTitle}
              shortDes={shortDes}
              setShortDes={setShortDes}
              image={image}
              setImage={setImage}
              selectImage={selectImage}
              onImageChange={onImageChange}
              logoSizeErr={logoSizeErr}
              updateTextDescription={updateTextDescription}
              editorState={editorState}
              desError={desError}
              readLength={readLength}
              setReadLength={setReadLength}
              onAddUpdateResource={onAddUpdateResource}
              isFeatured={isFeatured}
              setIsFeatured={setIsFeatured}
              resourceType={resourceType}
              setResourceType={setResourceType}
              sendNotification={sendNotification}
              setSendNotification={setSendNotification}
              youtubeVidId={youtubeVidId}
              setYoutubeVidId={setYoutubeVidId}
              isError={isError}
              setShowDelete={setShowDelete}
              location={location}
              onCancel={onCancel}
            />
          </Container>
          <DeleteInfoPopup
            show={showDelete}
            handleClose={() => setShowDelete(false)}
            deletePress={deleteJobById}
          />
        </div>
      </div>
      <div className="footerWrapper">
        <Footer />
      </div>
    </div>
  );
};

export default CreateEditResource;
