import React, { useEffect, useState, useRef } from 'react';
import { useDispatch } from 'react-redux';
import { ScreenContainer } from '../../components/ScreenContainer';
import Loader from '../../components/Loader';
import { userJobActions } from '../../store/user/userJobActions';
import { toast } from 'react-toastify';
import Amplitude from '../../utils/Amplitude';
import InfiniteScroll from 'react-infinite-scroll-component';
import { Row, Col } from 'react-bootstrap';
import SearchIcon from '@mui/icons-material/Search';
import ResourceCardMain from './ResourceCardMain';
import WestIcon from '@mui/icons-material/West';
import EastIcon from '@mui/icons-material/East';
import GetBoardwiseHeader from '../../utils/images/getBoardwiseHeader.svg';
import { useNavigate } from 'react-router-dom';
import Spinner from "react-bootstrap/Spinner";

function ResourceCenter() {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const scrollContainerRef = useRef(null);

  const [nextResourceTime, setNextResourceTime] = useState(null);
  const [search, setSearch] = useState('');
  const [hasMore, setHasMore] = useState(true);
  const [isLoading, setIsLoading] = useState(false);
  const [isLoadingMore, setIsLoadingMore] = useState(false);
  const [viewIntro, setViewIntro] = useState(true);
  const [allResources, setAllResources] = useState([]);
  const [introRes, setIntroRes] = useState([]);
  const userName = localStorage.getItem('displayName');
  const nameArray = userName.split(" ");

  useEffect(() => {
    window.scrollTo(0, 0);
    setIsLoading(true);
    dispatch(userJobActions.onGetResources(`next=${nextResourceTime}&&searchString=${search}`)).then(
      (res) => {
        setIsLoading(false);
        if (res?.success) {
          setNextResourceTime(res.data.next);
          setIntroRes(res.data.resources.slice(0, 3));
          setAllResources(res.data.resources);
        } else {
          toast.error(res?.message);
        }
      }
    );
    Amplitude('Views Resource Center');
  }, []);
  
  
  const toggleIntro = () => {
    setViewIntro(!viewIntro);
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
  const searchResources = (e) => {
    const x = e.code;
    if(x === "Enter") {
      setAllResources([])
      Amplitude('Search Resources', {
        search_string: `${search}`
      });
      window.scrollTo(0, 0);
        setIsLoading(true);
        dispatch(userJobActions.onGetResources(`next=null&&searchString=${search}`)).then(
          (res) => {
            setHasMore(true);
            setIsLoading(false);
            setNextResourceTime(null);
            if (res?.success) {
              setNextResourceTime(res.data.next);
              setAllResources(res.data.resources);
            } else {
              toast.error(res?.message);
            }
          }
        );    }   
  }
  const getMoreResources = () => {
    setIsLoadingMore(true);
    dispatch(userJobActions.onGetResources(`next=${nextResourceTime}&&searchString=${search}`)).then(
      (res) => {
        setIsLoadingMore(false);
        if (res?.success) {
          setNextResourceTime(res.data.next);
          if (res.data.resources.length > 0) Amplitude('load more resources');
          const allCombined = [...allResources, ...res.data.resources];
          setAllResources(allCombined);
          if (res.data.resources.length === 0) {
            setHasMore(false);
          }
        } else {
          toast.error(res?.message);
        }
      }
    );
  };

  return (
    <div>
      <ScreenContainer>
        {isLoading && <Loader />}
        <button className="mobileHeaderImg" onClick={()=>{
          Amplitude('Go to Homepage');
          navigate("/userdashboard")}}
        >
          <img src={GetBoardwiseHeader} alt="GetBoardwiseHeader" />
        </button>
        <div className="userResourceCenter">
          <InfiniteScroll
            dataLength={allResources?.length}
            next={getMoreResources}
            hasMore={hasMore}
            style={{ overflow: 'hidden' }}
          >
            {viewIntro && 
            <Row>
              <Col>
              <div className='resourceWrapper resourceWrapperIntro'>
                <div className='introArticlesText'>
                  <h1>Hello, <br />{nameArray[0]}</h1>
                  <span>Here are a few articles to get you started.</span>
                  <button onClick={toggleIntro}>Close intro</button>
                  <div>
                    <WestIcon sx={{color: "#1B76A9", fontSize: "30px", marginRight: "30px", cursor:"pointer"}} onClick={scrollLeft} />
                    <EastIcon sx={{color: "#1B76A9", fontSize: "30px", cursor:"pointer"}} onClick={scrollRight} />
                  </div>
                </div>
                <div className="resourceWrapper relatedArticles" ref={scrollContainerRef}>
                {introRes?.length > 0 &&
                  introRes.map((resource) => (
                    <ResourceCardMain key={resource.id} resource={resource} />
                  ))}
                  </div>
              </div>
              </Col>
              <hr />
            </Row>}
            <div className="resourceCenterWrapper">
              {!viewIntro && <div className='introArticlesClose'>
                  <h1>Hello, {nameArray[0]}</h1>
                  <button onClick={toggleIntro}><span>See</span> Intro Articles</button>
                </div>}
                <div className={!viewIntro && "allArticles"}>
            <div className="resourceSearch">
              {viewIntro && <div className="furtherReading"><h2>Further Reading</h2></div>}
            <div className={`searchBar`}><SearchIcon/>
            <input placeholder='Search the Resource Center' 
            type="search"
             onChange={(e) => {
              setSearch(e.target.value);
            }}
            onKeyDown={searchResources}/></div>
            </div>
                <div>
                  <div className={`resourceWrapper ${viewIntro && "resourceCenter"}`}>
                    {allResources?.length > 0 &&
                      allResources.map((resource) => (
                        <ResourceCardMain key={resource.id} resource={resource} />
                      ))}
                  </div>
                  <div className="loadMoreResources">
                    {isLoadingMore && nextResourceTime && <Spinner animation="border" />}
                  </div>
            </div>
            </div>
            </div>
            
          </InfiniteScroll>
        </div>
      </ScreenContainer>
    </div>
  );
}
export default ResourceCenter;
