import React from 'react';
import Card from 'react-bootstrap/Card';
import LocationIcon from '@mui/icons-material/NearMe';
import VerifiedIcon from '@mui/icons-material/Verified';
import { useNavigate } from 'react-router-dom';
import { GetBoardwise_logo } from '../../utils/image';
import PropTypes from "prop-types";

export default function JobBoard(props) {
  const navigate = useNavigate();
  const job = props.job;
  const industryAndSkill = job.industries[0] + ' - ' + job.skills[0];
  const jobTitles = job.titles.join(', ');
  const companyName =
    job.company.length > 36
      ? job.company.slice(0, 36) + '...'
      : job.company.slice(0, 36);
  const industryAndSkillText =
    industryAndSkill.length > 34
      ? industryAndSkill.slice(0, 27) + '...'
      : industryAndSkill.slice(0, 27);
  const jobTitle =
    jobTitles.length > 40
      ? jobTitles.slice(0, 40) + '...'
      : jobTitles.slice(0, 40);

  const getJobLocation = () => {
    if (job.locations?.length > 1) {
      return 'Multiple';
    }
    return job.locations?.length === 1
      ? job.locations[0]
      : job.locations[0] + ', ' + job.locations[1];
  };

  const handleJobClick = () => {
    if (job.company) {
      navigate(`/job/` + job.id, {
        state: {
          id: job.id,
        },
      });
    } 
    else {
      props.setJobModalShow(true);
    }
  }

  return (
    <Card
      className={`${job.isAlreadyApplied && "appliedJobCard"} jobCard`}
      onClick={() => handleJobClick()}
    >
      <Card.Body className="jobCardBody">
        <div className="jobCardHeader">
          <div>
          <div className="jobHeader">
          <Card.Img
            variant="left"
            alt={job.company}
            src={
              job.companyLogo === null || job.companyLogo.trim() === ''
                ? GetBoardwise_logo
                : job.companyLogo
            }
          />
          <div className="jobHeaderText">
            <p>
              <span className="jobCardHeaderCompany jobCardTextColor">
                {companyName || <span className="blurredName">Company Name</span>}
              </span>{' '}
              <br></br>
              <span className="jobHeaderIndustryAndSkillText jobCardTextColor">
                {industryAndSkillText}
              </span>
            </p>
          </div>
        </div>
          </div>
          <div className="jobCardTags">
          {job.isAlreadyApplied && (
            <div className="jobCardLocation appliedJob">
            <div className="jobSmallImageContainer">
              <VerifiedIcon
                sx={{ fontSize: 11, color: "#0F2B18" }}
                className="rounded mx-auto d-block"
              />
              <span>
                Applied
              </span>
            </div>
          </div>
          )}
          <div className="jobCardLocation">
          <div className="jobSmallImageContainer">
            <LocationIcon
              sx={{ fontSize: 11, color: "#3C7C50" }}
              className="rounded mx-auto d-block"
            />
            <span className="jobCardTextColor jobCardLocationText">
            {getJobLocation()}
          </span>
          </div>
          </div>
          </div>
        </div>
        

        <Card.Text className="jobTagLineHeight">
          <p className="jobCardTitleText jobCardTextColor">{jobTitle}</p>
          <p className="previewTextCard jobText">{job.previewText}</p>
          
        </Card.Text>
      </Card.Body>
    </Card>
  );
}

JobBoard.propTypes = {
  setJobModalShow: PropTypes.func,
  job: PropTypes.shape({
    industries: PropTypes.arrayOf(PropTypes.string),
    skills: PropTypes.arrayOf(PropTypes.string),
    titles: PropTypes.arrayOf(PropTypes.string),
    company: PropTypes.string,
    id: PropTypes.string,
    locations: PropTypes.arrayOf(PropTypes.string),
    isAlreadyApplied: PropTypes.bool,
    companyLogo: PropTypes.string,
    previewText: PropTypes.string
  })
}