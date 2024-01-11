import React, { useEffect, useState, useRef } from 'react';
import { AdminHeader } from '../../components/header/index';
import Footer from '../../components/footer/footer';
import { Container, Row, Col } from 'react-bootstrap';
import Loader from '../../components/Loader';
import { useNavigate, useLocation } from 'react-router-dom';
import { adminCourseActions } from '../../store/adminCourses/adminCourseActions';
import { useDispatch } from 'react-redux';
import Form from 'react-bootstrap/Form';
import {
  EditorState,
  convertFromRaw,
  convertToRaw,
  convertFromHTML,
  ContentState,
} from 'draft-js';
import draftToHtml from 'draftjs-to-html';
import { MultiSelect } from 'react-multi-select-component';
import Button from 'react-bootstrap/Button';
import { AiFillDelete } from 'react-icons/ai';
import { toast } from 'react-toastify';
import TextEditor from '../../components/TextEditor';
import DeleteInfoPopup from './deleteInfoPopup';

let allLocations = [];
let allSkills = [];
let allIndustory = [];
let allJobTitles = [];

const CreateEditJob = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const location = useLocation();

  const [showDelete, setShowDelete] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [selectedCategory, setSelectedCategory] = useState('');
  const [companyName, setCompanyName] = useState('');
  const [shortDes, setShortDes] = useState('');
  const [noRoles, setNoRoles] = useState('');
  const [siteLink, setSiteLink] = useState('');
  const [editorState, setEditorState] = useState(() =>
    EditorState.createEmpty()
  );
  const [logo, setLogo] = useState('');
  const [isError, setIsError] = useState(false);
  const [selectedIndustory, setSelectedIndustory] = useState([]);
  const [selectedSkills, setSelectedSkills] = useState([]);
  const [selectedLocation, setSelectedLocation] = useState([]);
  const [selectedTitle, setSelectedTitle] = useState([]);
  const [desError, setDesError] = useState(false);
  const [logoSizeErr, setLogoSizeErr] = useState(false);

  const checkIsEditOrAdd = () => {
    if (location.state.id !== 'new') {
      getJobDetail(location.state.id);
    } else {
      setIsLoading(false);
      setSelectedCategory('External');
    }
  };

  useEffect(() => {
    setIsLoading(true);
    dispatch(adminCourseActions.onGetIndustury()).then((inds) => {
      if (inds?.success) {
        allIndustory = inds.data;
      }
      dispatch(adminCourseActions.onGetSkills()).then((skil) => {
        if (skil?.success) {
          allSkills = skil.data;
        }
        dispatch(adminCourseActions.onGetTitles()).then((titles) => {
          if (titles?.success) {
            allJobTitles = titles.data;
          }
          dispatch(adminCourseActions.onGetLocation()).then((loc) => {
            if (loc?.success) {
              allLocations = loc.data;
              checkIsEditOrAdd();
            }
          });
        });
      });
    });
  }, []);

  const getJobDetail = (jobId) => {
    setIsLoading(true);
    dispatch(adminCourseActions.onGetJobById(jobId)).then((res) => {
      setIsLoading(false);
      if (res?.success && res.data) {
        setSelectedCategory(res.data.category);
        setCompanyName(res.data.company);
        setSiteLink(res.data.companyLink);
        setLogo(
          res.data.jobArtifacts.length > 0 ? res.data.jobArtifacts[0].url : ''
        );
        setShortDes(res.data.shortDescription);
        setNoRoles(res.data.noOfRole);
        const contentBlocks = convertFromHTML(res.data.description);
        const contentState = ContentState.createFromBlockArray(contentBlocks);
        const finalResult = convertToRaw(contentState);
        setEditorState(
          EditorState.createWithContent(convertFromRaw(finalResult))
        );
        setLocationValue(res.data.jobLocations);
        setIndustryValue(res.data.jobIndustry);
        setSkillValue(res.data.jobSkills);
        setTitleValue(res.data.jobTitle);
      }
    });
  };
  const setLocationValue = (jobLoc) => {
    let selectedLoc = [];
    jobLoc.forEach((item) => {
      const result = allLocations.find((x) => x.value == item.value);
      selectedLoc.push(result);
    });
    setSelectedLocation(selectedLoc);
  };
  const setIndustryValue = (jobLoc) => {
    let selectedInd = [];
    jobLoc.forEach((item) => {
      const result = allIndustory.find((x) => x.value == item.value);
      selectedInd.push(result);
    });
    setSelectedIndustory(selectedInd);
  };
  const setSkillValue = (jobLoc) => {
    let selectedSkl = [];
    jobLoc.forEach((item) => {
      const result = allSkills.find((x) => x.value == item.value);
      selectedSkl.push(result);
    });
    setSelectedSkills(selectedSkl);
  };
  const setTitleValue = (jobTitle) => {
    let selectedTitles = [];
    jobTitle.forEach((item) => {
      const result = allJobTitles.find((x) => x.value == item.value);
      selectedTitles.push(result);
    });
    setSelectedTitle(selectedTitles);
  };
  const updateTextDescription = async (state) => {
    await setEditorState(state);
    const data = convertToRaw(editorState.getCurrentContent());
    console.log(data);
    setDesError(false);
  };

  const onImageChange = (event) => {
    if (event.target.files?.[0]) {
      if (event.target.files[0].size <= 5242880) {
        setLogoSizeErr(false);
        let img = event.target.files[0];
        if (location.state.id !== 'new') {
          const jobImgPayload = {
            id: location.state.id,
            file: img,
          };
          setIsLoading(true);
          dispatch(adminCourseActions.onUploadImage(jobImgPayload)).then(
            (res) => {
              setIsLoading(false);
              if (res?.success) {
                setLogo(res.data?.url);
              }
            }
          );
        } else {
          setLogo(img);
        }
      } else {
        setLogoSizeErr(true);
      }
    }
  };

  const updateRes = (res) => {
    if (res?.success) {
      navigate(-1);
      toast.success('Job updated successfully');
    } else {
      toast.error(res?.message);
    }
  };

  const onAddUpdateJob = async (e) => {
    e.preventDefault();
    const value = draftToHtml(convertToRaw(editorState.getCurrentContent()));
    if (
      selectedCategory !== '' &&
      companyName !== '' &&
      shortDes !== '' &&
      shortDes.length <= 200 &&
      logo !== '' &&
      selectedLocation.length > 0 &&
      selectedIndustory.length > 0 &&
      selectedSkills.length > 0 &&
      selectedTitle.length > 0 &&
      siteLink !== '' &&
      noRoles !== '' &&
      value !== '' &&
      value.length > 8
    ) {
      setIsError(false);
      setIsLoading(true);
      const jobPayload = {
        company: companyName,
        file: logo,
        companyLink: siteLink,
        noOfRole: parseInt(noRoles),
        shortDescription: shortDes,
        description: value,
        category: selectedCategory,
        jobLocations: selectedLocation,
        jobSkills: selectedSkills,
        jobIndustry: selectedIndustory,
        jobTitles: selectedTitle,
      };
      if (location.state.id !== 'new') {
        dispatch(
          adminCourseActions.onUpdateJob(location.state.id, jobPayload)
        ).then((res) => {
          setIsLoading(false);
          updateRes(res);
        });
      } else {
        dispatch(adminCourseActions.onAddAdminJob(jobPayload)).then((res) => {
          setIsLoading(false);
          if (res?.success) {
            navigate(-1);
            toast.success('Job created successfully');
          } else {
            toast.error(res?.message);
          }
        });
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
  const selectLogo = (value) => {
    if (location.state.id !== 'new') {
      return value;
    } else {
      return URL.createObjectURL(value);
    }
  };

  const isEmpty = (value, label) => {
    if (isError && value === '') {
      return (
        <p className="errorTxt">Please enter {label}.</p>
      )
    }
  }

  const jobCreateEditForm = () => {
    return (
      <div className="editorPageWidth">
        {isLoading && <Loader />}

        <Row>
          <Col sm={10}>
            <Form>
              <Row>
                <Col xs={6}>
                  <h3>
                    {location.state.id !== 'new' ? 'Update Job' : 'Create Job'}
                  </h3>
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
                <Form.Label>Category</Form.Label>
                <div className="jobCatWrapper">
                  <span className="externalCat">
                    <input
                      type="radio"
                      id="cat1"
                      name="category"
                      value="External"
                      checked={selectedCategory === 'External'}
                      onChange={(e) => setSelectedCategory(e.target.value)}
                    />
                    <span>External</span>
                  </span>
                  <span className="externalCat">
                    <input
                      type="radio"
                      id="cat2"
                      name="category"
                      value="Exclusive"
                      checked={selectedCategory === 'Exclusive'}
                      onChange={(e) => setSelectedCategory(e.target.value)}
                    />
                    <span>Exclusive</span>
                  </span>
                </div>
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Company name</Form.Label>
                <Form.Control
                  type="text"
                  id="company"
                  placeholder="Enter Company Name"
                  value={companyName}
                  required={true}
                  onChange={(e) => {
                    setCompanyName(e.target.value);
                  }}
                />
                {isEmpty(companyName, "company name")}
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Description</Form.Label>
                <div className="ckEditorWrapper">
                  <TextEditor
                    updateTextDescription={updateTextDescription}
                    editorState={editorState}
                  />
                </div>
                {desError ? (
                  <p className="errorTxt">Please enter job description.</p>
                ) : null}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Preview text</Form.Label>
                <Form.Control
                  as="textarea"
                  rows={3}
                  id="previewText"
                  placeholder="Enter Preview Text"
                  value={shortDes}
                  required={true}
                  onChange={(e) => {
                    setShortDes(e.target.value);
                  }}
                />
                <p className="helpingTxt">
                  {'Maximum length could be 200 characters. '}
                  {shortDes.length}
                </p>
                {isEmpty(shortDes, "preview text")}
                {shortDes.length > 200 ? (
                  <p className="errorTxt">
                    Preview text cannot exceed from 200 characters
                  </p>
                ) : null}
              </Form.Group>
              <Form.Group className="mb-3 inputRoles">
                <Form.Label>Number of roles</Form.Label>
                <Form.Control
                  type="number"
                  id="noRole"
                  placeholder="Enter number of roles"
                  value={noRoles}
                  required={true}
                  onChange={(e) => {
                    setNoRoles(e.target.value);
                  }}
                />
                {isEmpty(noRoles, "number of roles")}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Logo</Form.Label>
                <div>
                  {logo !== '' ? (
                    <div className="logoImg">
                      <img src={selectLogo(logo)} alt="logo" />
                      <span>
                        <AiFillDelete
                          className="deleteImgIcon"
                          onClick={() => setLogo('')}
                        />
                      </span>
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
                {isError && logo === '' ? (
                  <p className="errorTxt">Please select image</p>
                ) : null}
                {logoSizeErr === true ? (
                  <p className="errorTxt">Maximum image size can be 5 MB</p>
                ) : null}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Select location</Form.Label>
                <MultiSelect
                  options={allLocations}
                  value={selectedLocation}
                  onChange={setSelectedLocation}
                  labelledBy="Select"
                  hasSelectAll={false}
                />
                {isError && selectedLocation.length < 1 ? (
                  <p className="errorTxt">Please select location.</p>
                ) : null}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Select skills</Form.Label>
                <MultiSelect
                  options={allSkills}
                  value={selectedSkills}
                  onChange={setSelectedSkills}
                  labelledBy="Select"
                  hasSelectAll={false}
                />
                {isError && selectedSkills.length < 1 ? (
                  <p className="errorTxt">Please select skills.</p>
                ) : null}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Select industry</Form.Label>
                <MultiSelect
                  options={allIndustory}
                  value={selectedIndustory}
                  onChange={setSelectedIndustory}
                  labelledBy="Select"
                  hasSelectAll={false}
                />
                {isError && selectedIndustory.length < 1 ? (
                  <p className="errorTxt">Please select industry</p>
                ) : null}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Select job title</Form.Label>
                <MultiSelect
                  options={allJobTitles}
                  value={selectedTitle}
                  onChange={setSelectedTitle}
                  labelledBy="Select"
                  hasSelectAll={false}
                />
                {isError && selectedTitle.length < 1 ? (
                  <p className="errorTxt">Please select job title</p>
                ) : null}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Link/Url</Form.Label>
                <Form.Control
                  type="text"
                  id="link"
                  placeholder="Enter link or url"
                  value={siteLink}
                  required={true}
                  onChange={(e) => {
                    setSiteLink(e.target.value);
                  }}
                />
                {isEmpty(siteLink, "website link")}
              </Form.Group>
              <br />
              <Button
                id="submitCourse"
                variant="primary"
                type="submit"
                onClick={onAddUpdateJob}
              >
                Submit
              </Button>
            </Form>
          </Col>
        </Row>
      </div>
    );
  };

  const deleteJobById = () => {
    setIsLoading(true);
    dispatch(adminCourseActions.onDeleteJob(location.state.id)).then((data) => {
      setIsLoading(false);
      if (data?.success) {
        navigate(-1);
        toast.success('Job deleted successfully');
      } else {
        toast.error(data?.message);
      }
    });
  };

  const scrollToView = useRef(null);
  return (
    <div>
      <AdminHeader />
      <div className="bodyWrapper">
        <div className="whiteBackground">
          <Container className="projectDetail" ref={scrollToView}>
            {jobCreateEditForm()}
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

export default CreateEditJob;
