import React, { useEffect, useState, useRef } from 'react';
import { UserHeader } from '../../components/header/index';
import MobileNav from '../../components/footer/mobileNav';
import { useDispatch } from 'react-redux';
import { useParams, useNavigate } from 'react-router-dom';
import { userJobActions } from '../../store/user/userJobActions';
import { toast } from 'react-toastify';
import Loader from '../../components/Loader';
import Row from 'react-bootstrap/Row';
import Container from 'react-bootstrap/Container';
import Col from 'react-bootstrap/Col';
import parse from 'html-react-parser';
import WestIcon from '@mui/icons-material/West';
import EastIcon from '@mui/icons-material/East';
import ResourceCardMain from './ResourceCardMain';
import { GetBoardwise_logo } from '../../utils/image';
import ArrowLeftIcon from '../../utils/images/Polygon2.png';
import YouTube from "react-youtube";
import Amplitude from '../../utils/Amplitude';

const ResourceDetail = () => {
  const { url } = useParams();
  const navigate = useNavigate();
  const scrollContainerRef = useRef(null);
  const dispatch = useDispatch();
  const [allResources, setAllResources] = useState([]);
  const breakpoint = 1000;
  const breakpointMob = 767;
  const [width, setWidth] = useState(window.innerWidth);
  const [isLoading, setIsLoading] = useState(true);
  const [resourceData, setResourceData] = useState({
    id: '',
    length: '',
    title: '',
    previewText: '',
    date: '',
    media: {},
    description: '',
    resourceType: 1,
    videoLink: '',
  });
  const [isMember, setIsMember] = useState(false);
  const [videoResource, setVideoResource] = useState(false);
  const [remainingArticles, setRemainingArticles] = useState(0);

  const getRelatedResouces = () => {
    dispatch(userJobActions.onGetResources(`next=null`)).then(
      (res) => {
        setIsLoading(false);
        if (res?.success) {
          setAllResources(res.data.resources);
        } else {
          toast.error(res?.message);
        }
      }
    );
  }

  const scrollLeft = () => {
    if (scrollContainerRef.current) {
      scrollContainerRef.current.scrollLeft -= 200; // Adjust the scroll amount as needed
    }
  };

  const scrollRight = () => {
    if (scrollContainerRef.current) {
      scrollContainerRef.current.scrollLeft += 200; // Adjust the scroll amount as needed
    }
  }
  const clickShare = () => {
    Amplitude("Share Resource");
    navigator.clipboard.writeText(window.location.href);
    toast.success("Link copied to clipboard!");
  }

  const dateAndAuthor = (resourceData) => (
    <div className="dateAndAuthor">
      <div style={{display:"flex", justifyContent:"space-between"}}>
        <div className="resAuthor">
          <img src={GetBoardwise_logo} alt="author img" />
          <div>Boardwise
          </div>
        </div>
        {isMember && 
        <div className="resDetClap">
          <button className='shareResource' onClick={clickShare}>Share</button>
        </div>}
      </div>
    </div>
  )
  const opts = {
    height: '272',
    width: '484',
  };
  const opts2 = {
    width: '100%',
    height: '300'
  };

  const ytOptions = width > 1000 ? opts : opts2;

  useEffect(() => {
    setIsLoading(true);
    window.scrollTo(0, 0);
    const handleWindowResize = () => setWidth(window.innerWidth);
    window.addEventListener('resize', handleWindowResize);
    dispatch(userJobActions.onGetResourceByUrl(url)).then((res) => {
      const { resource, user } = res.data;
      if (user.isPaidUser === false) {
        setIsMember(false);
        setRemainingArticles(user.FreeResourcesAvailable);
        if (resource.resourceType===2) {
          setVideoResource(true);
        }
      }
      else {
        setIsMember(true);
      }
      if (res?.success) {
        setResourceData({
          id: resource.id,
          length: resource.length,
          title: resource.title,
          previewText: resource.preview,
          date: resource.time,
          media: resource.media[0],
          description: resource.description,
          resourceType: resource.resourceType,
          videoLink: resource.videoLink,
        });
        setIsLoading(false);
        Amplitude('view resource', {
          'resource title': resource.title,
        });
      } else {
        setIsLoading(false);
        if (user.message === 'Un paid member') {
          window.location.href = user.data.url + `/membership/`;
          toast.error(user.message);
        } else {
          toast.error(res?.message);
        }
      }
    });
    getRelatedResouces();
  }, [window.location.pathname]);

  return (
    <div>
      {width >= breakpointMob && <UserHeader />}
      {isLoading ? (<Loader />) : (
        <div>
          <div className="resourceHeader">
            <span className="backButtonCont">
              <button
                className="backButton"
                onClick={() => {
                  navigate('/resourcecenter');
                }}>
                  <span className="marginRight10">
                    <img src={ArrowLeftIcon} alt="leftArrow" />
                  </span>Back to articles</button>
            </span>
          </div>
          {videoResource ? 
          <div className="videoResDiv">
            This resource can only be viewed by paid users.
          </div> : 
          <Container className="resourceContainer">
            <Row>
              <Col className="col-12 col-lg-6">
                <div className="resDetBody">
                  <p className="resDetTitle">{resourceData.title}</p>
                  {width >= breakpoint && dateAndAuthor(resourceData)}
                </div>
                </Col>
                {resourceData.resourceType === 1 ? <Col className="col-12 col-lg-6 resourceImage">
                  <img
                    className="resDetImg"
                    src={resourceData.media.url}
                    alt="resource-cover"
                  />
                </Col>: 
                <Col className="col-12 col-lg-6 resourceImage">
                <YouTube videoId={resourceData.videoLink} opts={ytOptions} id="video"/>
              </Col>
              }
                {width < breakpoint &&
                  <div className="resDetBody">
                  {dateAndAuthor(resourceData)}
                  </div>
                }
              <hr />
                </Row>
                <Row>
                  <Col>
                  <p className="resDetail">{parse(resourceData.description)}</p>
                  {isMember && <div className="resDetClap resDetClapEnd">
                  <button className='shareResource' onClick={clickShare}>Share</button>
                </div>}
              </Col>
            </Row>
            <hr />
            {(isMember || (!isMember && remainingArticles)) ? <div>
            <div className="relatedArticlesTitle">
              <div>
                Related Articles
              </div>
              <div className="relatedResourceNav">
                <WestIcon sx={{color: "#1B76A9", fontSize: "30px", marginRight: "30px", cursor:"pointer"}} onClick={scrollLeft} />
                <EastIcon sx={{color: "#1B76A9", fontSize: "30px", cursor:"pointer"}}onClick={scrollRight} />
              </div>
            </div>
            <Row>
              <Col>
              <div className="resourceWrapper relatedArticles" ref={scrollContainerRef}>
                {allResources?.length > 0 &&
                  allResources.map((resource) => (
                    <ResourceCardMain key={resource.id} resource={resource} />
                  ))}
                </div>
              </Col>
            </Row>
            </div> : null}
          </Container>
          }
          </div>
        )}
        {width < breakpointMob && <MobileNav />}
      {!isLoading && !isMember && 
      <div className={`nonMemberResource ${remainingArticles && 'nonMemberRes'}`}>
        <h2>
        {remainingArticles ? `You have ${remainingArticles} remaining articles!` : "You have run out of complimentary articles" }
        </h2>
        {!remainingArticles ?
        <p>Upgrade your membership to get unlimited access to our Resource Centre and enjoy other exclusive benefits, including access to exclusive job roles.</p>
        : null}
        <div>
          <button className="saveButton" onClick={()=>{
            Amplitude("Upgrade on Resource");
            navigate('/pricing');
            }}>
            Upgrade Now
          </button>
          <p className='alreadyUpgraded'>
            Already Upgraded? <button onClick={()=>navigate('/login')}>Login</button>
          </p>
        </div>
      </div>}
    </div>
  );
};

export default ResourceDetail;
