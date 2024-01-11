import React, { useEffect, useState } from 'react';
import { ScreenContainer } from '../../components/ScreenContainer';
import { useParams, useNavigate } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { userJobActions } from '../../store/user/userJobActions';
import LocationIcon from '@mui/icons-material/NearMe';
import PublicIcon from '@mui/icons-material/Public';
import CloseIcon from '@mui/icons-material/Close';
import Loader from '../../components/Loader';
import parse from 'html-react-parser';
import Amplitude from '../../utils/Amplitude';
import Modal from 'react-bootstrap/Modal';
import { emailRegx } from '../../utils/validationCheck';
import ApplyJobModal from './applyJobModal';
import ArrowLeftIcon from '../../utils/images/Polygon2.png';
import VerifiedIcon from '@mui/icons-material/Verified';
import PropTypes from "prop-types";




function JobSubmittedModal(props) {
  return (
    <Modal
      {...props}
      aria-labelledby="contained-modal-title-vcenter"
      centered
      className="applyJobModal"
      dialogClassName="jobSubmittedModal"
    >
      <Modal.Body>
        <CloseIcon
          onClick={props.onHide}
          className="closeButton"
          sx={{ fontSize: 20 }}
        />
        <p className="jobSubmittedText">Application successfully submitted</p>
      </Modal.Body>
    </Modal>
  );
}

JobSubmittedModal.propTypes = {
  onHide: PropTypes.func
}


export default function JobDetail() {
  const { jobId } = useParams();
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const [jobDetails, setJobDetails] = useState({
    companyLink: '',
    locations: [],
    industries: [],
    skills: [],
    description: '',
    companyLogo: '',
    titles: [],
  });
  const [isLoading, setIsLoading] = useState(false);
  const [jobApplyModalShow, setJobApplyModalShow] = useState(false);
  const [jobSubmittedShow, setJobSubmittedShow] = useState(false);
  const [returnSuccess, setReturnSuccess] = useState(true);
  const [jobApplicationSuccess, setJobApplicationSuccess] = useState(false);
  const [jobApplyError, setJobApplyError] = useState({
    status: false,
    value: '',
  });

  const [fullName, setFullName] = useState('');
  const [email, setEmail] = useState('');
  const [valueToAdd, setValueToAdd] = useState('');
  const [cv, setCv] = useState('');

  const [isError, setIsError] = useState(true);
  const [isValidEmail, setIsValidEmail] = useState(true);
  const [isValidFile, setIsValidFile] = useState({ status: false, value: '' });

  const hiddenFileInput = React.useRef(null);

  useEffect(() => {
    setIsLoading(true);
    dispatch(userJobActions.onGetUserJobById(jobId))
      .then((res) => {
        setReturnSuccess(res.success);
        setJobDetails(res.data);
        setJobApplicationSuccess(res.data.isAlreadyApplied);
        Amplitude('View Job Detail', {
          companyName: res.data.company,
          jobName: res.data.titles.join(', '),
        });
        setIsLoading(false);
      })
      .catch(() => {
        setIsLoading(false);
      });
  }, []);

  useEffect(() => {
    if (fullName === '' || email === '' || valueToAdd === '' || cv === '') {
      setIsError(true);
    } else {
      setIsError(false);
    }
  }, [fullName, email, valueToAdd, cv]);

  const handleFileUpload = (file) => {
    setCv(file);
    if (file.size >= 5242880) {
      setIsValidFile({
        status: false,
        value: 'File size is exceeding the limit',
      });
      setIsError(true);
    } else if (
        file.type == 'application/pdf' ||
        file.type ==
          'application/vnd.openxmlformats-officedocument.wordprocessingml.document' ||
        file.type == 'application/msword'
      ) {
        setIsValidFile({ status: true, value: '' });
        setIsError(false);
    } 
    else {
        setIsValidFile({
          status: false,
          value: 'Invalid file format. Only PDF and DOC format is allowed',
        });
        setIsError(true);
    }
  };

  const validateEmail = (value) => {
    const testEmail = emailRegx.exec(String(value).toLowerCase());
    if (testEmail !== null) {
      setIsValidEmail(true);
    } else {
      setIsValidEmail(false);
    }
  };

  const handleClick = (event) => {
    if (cv) {
      event.preventDefault();
    } else {
      hiddenFileInput.current.click();
    }
  };

  const handleChange = (event) => {
    const fileUploaded = event.target.files[0];
    handleFileUpload(fileUploaded);
  };
  const navigateToCompany = (companyURL) => {
    Amplitude('Link to Company');
   if(!companyURL.startsWith('https://'))
   {
    companyURL="https://"+companyURL
   }
    window.open(companyURL ,'_blank');
  };
  const handleSendApplication = () => {
    setIsLoading(true);
    const jobData = {
      name: fullName,
      email,
      description: valueToAdd,
      file: cv,
      jobId: jobId,
    };
    dispatch(userJobActions.onApplyJob(jobData))
      .then((res) => {
        if (res.success) {
          setJobApplyModalShow(false);
          setJobApplicationSuccess(true);
          setJobSubmittedShow(true);
          setFullName('');
          setEmail('');
          setValueToAdd('');
          setCv('');
          Amplitude('Apply Job Success', {
            companyName: jobDetails.company,
            email:jobData.email,
            cv_file:jobData.file,
            value_description: jobData.description,
          });
        } else {
          setJobApplyError({ status: true, value: res.message });
        }
        setIsLoading(false);
      })
      .catch(() => {
        setIsLoading(false);
      });
  };

  if (returnSuccess) {
    return (
      <div>
        <ScreenContainer className="noPaddingMargin">
          {isLoading ? (
            <Loader />
          ) : (
            <div className="noPaddingMargin">
              <div>
                <div className="headingAlignment">
                  <span className="backButtonCont">
                    <button
                      onClick={() => {
                        navigate('/jobs-board');
                      }}
                      className="backButton"
                    >
                      <span className="marginRight10">
                        <img src={ArrowLeftIcon} alt="leftArrow" />
                      </span>Back to roles</button>
                  </span>
                </div>
              </div>
              <div className="jobDetailContainer">
              <p className="jobDetailHeading jobDetailHeadingMobile">{jobDetails.company}</p>
                    <div>
                      <p className="jobDetailSubHeading jobDetailSubHeadingMobile">
                        {`${jobDetails.industries.join(
                          ', '
                        )} - ${jobDetails.skills.join(', ')}`}
                      </p>
                    </div>
                <div className="jobDetailsImgSection">
                  <section>
                        <div className="marginVertical20">
                          <img
                            src={
                              jobDetails.companyLogo.trim() === ''
                                ? '/no_image.svg'
                                : jobDetails.companyLogo
                            }
                            alt={jobDetails.company}
                            className="companyImg shadow bg-white"
                          />
                        </div>
                        <div className="linksAndApply">
                        <div className="locAndLink">
                          <div className="displayFlex">
                            <span className="jobSmallImageContainer jobDetailImg">
                              <LocationIcon
                                sx={{ fontSize: 12, color: "#3C7C50" }}
                              />
                            </span>
                            <span className="jobHeaderCompanyText jobCardTextColor jobMarginLeft6">
                              {jobDetails.locations.join(', ')}
                            </span>
                          </div>
  
                          <div className="displayFlex marginLeft10">
                            <span className="jobSmallImageContainer jobDetailImg">
                              <PublicIcon 
                              sx={{ fontSize: 12, color: "#3C7C50" }} />
                            </span>
                            <button
                              onClick={() => navigateToCompany(jobDetails.companyLink)}
                              className="jobHeaderCompanyText jobCardTextColor jobMarginLeft6 backButton"
                            >
                              <span className='longText'>{jobDetails.companyLink.replace(/^https?:\/\//, '')}</span>
                            </button>
                          </div>
                        </div>
                    {jobApplicationSuccess ? (
                      <div className="jobDetailApplied">
                        <VerifiedIcon
                          sx={{ fontSize: 14, color: "#0F2B18" }}
                        />
                        <span>
                          Applied
                        </span>
                      </div>
                    ) : 
                    <button
                      onClick={() => {
                        setJobApplyModalShow(true);
                        Amplitude('Apply Job', {
                          companyName: jobDetails.company,
                        });
                      }}
                      className="applyButtonStyle applyText"
                      disabled={jobApplicationSuccess}
                      >
                        Apply Now
                    </button>
                  }
                  </div>
                  </section>
                </div>
                <div className="jobDescriptionSection">
                  <section className="jobDetailsSection">
                      <p className="jobDetailHeading jobDetailCompany">{jobDetails.company}</p>
                    <div>
                      <p className="jobDetailSubHeading">
                        {`${jobDetails.industries.join(
                          ', '
                        )} - ${jobDetails.skills.join(', ')}`}
                      </p>
                    </div>
                    <p className="jobDetailHeading">
                      {jobDetails.titles.join(', ')}
                    </p>
                    <p className="jobDetail">{parse(jobDetails.description)}</p>
                  </section>
                </div>
              </div>
            </div>
          )}
        </ScreenContainer>
        <ApplyJobModal
          show={jobApplyModalShow}
          onHide={() => {
            setJobApplyModalShow(false);
            Amplitude('Apply Job Failed');
          }}
          fullName={fullName}
          setFullName={setFullName}
          email={email}
          setEmail={setEmail}
          valueToAdd={valueToAdd}
          setValueToAdd={setValueToAdd}
          cv={cv}
          setCv={setCv}
          handleFileUpload={handleFileUpload}
          isError={isError}
          setIsError={setIsError}
          handleClick={handleClick}
          handleChange={handleChange}
          hiddenFileInput={hiddenFileInput}
          isValidFile={isValidFile}
          handleSendApplication={handleSendApplication}
          validateEmail={validateEmail}
          isValidEmail={isValidEmail}
          jobApplyError={jobApplyError}
          setJobApplyError={setJobApplyError}
        />
  
        <JobSubmittedModal
          show={jobSubmittedShow}
          onHide={() => {
            setJobSubmittedShow(false);
            navigate('/jobs-board')}}
        />
      </div>
    );
  }
  else {
    navigate('/jobs-board');
  }
}
