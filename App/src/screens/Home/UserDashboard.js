import React, { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { ScreenContainer } from '../../components/ScreenContainer';
import { Row, Col } from 'react-bootstrap';
import JobBoard from '../Users/jobBoard';
import Loader from '../../components/Loader';
import { dashboardActions } from '../../store/dashboard/dashboardActions';
import { toast } from 'react-toastify';
import Amplitude from '../../utils/Amplitude';
import OpenInNew from '@mui/icons-material/OpenInNew';
import ResourceCard from './resourceCard';
import { useNavigate } from 'react-router-dom';
import JobsBoardModal from './jobsBoardModal';
import GetBoardwiseHeader from '../../utils/images/getBoardwiseHeader.svg';
import CircleIcon from '@mui/icons-material/Circle';

const navigatePrivacyPolicy = () => {
  Amplitude('View Privacy Policy');
  window.open('https://www.getboardwise.com/privacy-policy', '_blank');

}
const navigateTandC = () => {
  Amplitude('View T&Cs');
  window.open('https://www.getboardwise.com/terms-conditions', '_blank');

}

function UserDashboard() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const [dashboardJobs, setDashboardJobs] = useState([]);
  const [dashboardResources, setDashboardResources] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [jobModalShow, setJobModalShow] = useState(false);

  const oneToOne = (<div className="targetPlanBlock">
    <p className='targetPlanHeading'>1-2-1 Target Plan</p>
    <div>
      <p style={{ fontSize: "14px" }}>Here at Boardwise we do not just rely on a jobs board and a network of businesses,
        if you know the industry you want to target we will do the rest.</p>
      <button onClick={() => window.open("https://www.getboardwise.com", "_blank")}>
        Learn More <span><OpenInNew /></span>
      </button>
    </div>
  </div>)

  useEffect(() => {
    setIsLoading(true);
    dispatch(dashboardActions.onGetDashboard()).then((res) => {
      setIsLoading(false);
      if (res?.success) {
        setDashboardJobs(res.data.jobs);
        setDashboardResources(res.data.resources);
      } else {
        toast.error(res?.message);
        if (res?.message == 'Un paid member') {
          window.location.href = res.data.url + `/membership/`;
        }
      }
    });
    Amplitude('Views Homepage');
  }, []);

  return (
    <div>
      <ScreenContainer>
        {isLoading && <Loader />}
          <img className="mobileHeaderImg" src={GetBoardwiseHeader} alt="GetBoardwiseHeader" />
        <div className="dashboardSection">
          <div className=''>
            <div className='dashboardHeadings'>
              <h2>Recently Featured Jobs</h2>
              <button onClick={() => navigate('/jobs-board')}><span>See</span> All Jobs</button>
            </div>
          </div>
          <div className='jobsBoardDisplay'>
            {dashboardJobs?.length > 0 &&
              dashboardJobs.map((job) => (
                <JobBoard key={job.id} job={job} setJobModalShow={setJobModalShow} />
              ))}
          </div>
        </div>
        <hr className='d-none d-sm-block' />
        <Col className='col-lg-10 col-md-12 targetPlanMobile'>
          {oneToOne}
        </Col>
        <div className="dashboardSection">
          <div className=''>
            <div className='dashboardHeadings'>
              <h2>Our Latest Articles</h2>
              <button onClick={() => navigate('/resourcecenter')}><span>See</span> All Articles</button>
            </div>

          </div>
          <div className='resourceBoardDisplay'>
            {dashboardResources?.length > 0 &&
              dashboardResources.map((resource) => (
                <ResourceCard key={resource.id} resource={resource} />
              ))}
          </div>
        </div>
        <hr className='d-none d-sm-block' />
        <Row className="dashboardSection">
          <Col className='col-2'>
          </Col>
          <Col className='col-lg-10 col-md-12 targetPlan'>
            {oneToOne}
          </Col>
        </Row>
        <div className="TCmobile">
          <button onClick={() => navigateTandC()}>Terms and Conditions</button>
          <CircleIcon sx={{ fontSize: "8px" }} />
          <button onClick={() => navigatePrivacyPolicy()}>Privacy Policy</button>
        </div>
      </ScreenContainer>
      <JobsBoardModal
        show={jobModalShow}
        onHide={() => setJobModalShow(false)}
      />
    </div>
  );
}
export default UserDashboard;
