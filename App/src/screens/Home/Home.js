import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ScreenContainer } from '../../components/ScreenContainer';
import JobBoard from '../Users/jobBoard';
import Loader from '../../components/Loader';
import { userJobActions } from '../../store/user/userJobActions';
import { toast } from 'react-toastify';
import Amplitude from '../../utils/Amplitude';
import { SearchFilter } from './SearchFilter';
import FilterAltIcon from '@mui/icons-material/FilterAlt';
import SearchIcon from '@mui/icons-material/Search';
import InfiniteScroll from 'react-infinite-scroll-component';
import JobsBoardModal from './jobsBoardModal';
import GetBoardwiseHeader from '../../utils/images/getBoardwiseHeader.svg';
import { useNavigate } from 'react-router-dom';
import MdClose from '../../utils/images/Vector.svg';

function Home() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const {
    allJobs,
    totalRolesCount,
    sortType,
    selectedSkillIds,
    selectedIndustrieIds,
    selectedLocationIds,
    selectedTitleIds,
    lastSearch,
    searchString,
    selectedSkills,
    selectedIndustries,
    selectedLocations,
    selectedTitles,
  } = useSelector((state) => state.UserJobReducer);

  const [nextJobsTime, setNextJobsTime] = useState(null);
  const [hasMore, setHasMore] = useState(true);
  const [isLoading, setIsLoading] = useState(false);
  const [showFilter, setShowFilter] = useState(false);
  const [search, setSearch] = useState(searchString || '');
  const [jobModalShow, setJobModalShow] = useState(false);
  const breakpoint = 600;
  const [width, setWidth] = useState(window.innerWidth);
  let nf = new Intl.NumberFormat('en-US');
  console.log(selectedSkills,
    selectedIndustries,
    selectedLocations,
    selectedTitles)

  useEffect(() => {
    setIsLoading(true);
    const handleWindowResize = () => setWidth(window.innerWidth);
    window.addEventListener('resize', handleWindowResize);
    const data = setupGetJobsData();
    dispatch(userJobActions.getJobsByFilters(data, false)).then((res) => {
      setIsLoading(false);
      if (res?.success) {
        setNextJobsTime(res.data.dateTime);
      } else {
        toast.error(res?.message);
        if (res?.message == 'Un paid member') {
          window.location.href = res.data.url + `/membership/`;
        }
      }
    });
    Amplitude('Views Jobs Board');
  }, []);
  const onGetJobData = (isClear) => {
    const payload = {
      sort: sortType,
      skills: isClear ? [] : selectedSkillIds,
      titles: isClear ? [] : selectedTitleIds,
      industries: isClear ? [] : selectedIndustrieIds,
      locations: isClear ? [] : selectedLocationIds,
      lastDate: null,
      searchString: searchString,
    };
    setIsLoading(true);
    dispatch(userJobActions.getJobsByFilters(payload, false)).then((res) => {
      setIsLoading(false);
      if (!res?.success) {
        toast.error(res.message);
      }
      setNextJobsTime(res.data.dateTime);
    });
  };

  let setupGetJobsData = () => {
    return {
      sort: sortType,
      skills: selectedSkillIds,
      titles: selectedTitleIds,
      industries: selectedIndustrieIds,
      locations: selectedLocationIds,
      lastDate: nextJobsTime,
      searchString: search,
    };
  };

  let setupGetJobsDataForSort = (sortValue, isLastSearch) => {
    return {
      sort: sortValue,
      skills: isLastSearch ? lastSearch.selectedSkillIds : selectedSkillIds,
      titles: isLastSearch ? lastSearch.selectedTitleIds : selectedTitleIds,
      industries: isLastSearch
        ? lastSearch.selectedIndustrieIds
        : selectedIndustrieIds,
      locations: isLastSearch
        ? lastSearch.selectedLocationIds
        : selectedLocationIds,
      lastDate: null,
      searchString: search,
    };
  };

  const sortAmplitude = (jobType) => {
    Amplitude('sort jobs', {
      jobType: `${jobType}`,
    });
  };

  const getMoreJobs = () => {
    const data = setupGetJobsData();
    if (allJobs.length >= 30) {
      setIsLoading(true);
      dispatch(userJobActions.getJobsByFilters(data, true)).then((res) => {
        setIsLoading(false);
        if (res?.success) {
          const allCombineJobs = [...allJobs, ...res.data.jobs];
          dispatch(userJobActions.onChange('allJobs', allCombineJobs));
          setNextJobsTime(res.data.dateTime);
          if (res.data.noOfJobs === 0) {
            setHasMore(false);
          }
        }
      });
    } else {
      setHasMore(false);
    }
  };

  const searchRoles = (e) => {
    const x = e.code;
    if(x == "Enter") {
      Amplitude('Search Job', {
        search_string: `${search}`
      });
        setSearch(e.target.value);
        setIsLoading(true);
        dispatch(userJobActions.onChange('searchString', search));
        let data = setupGetJobsData();
        data.lastDate = null;
        dispatch(userJobActions.getJobsByFilters(data, false)).then((res) => {
          setIsLoading(false);
          if (!res?.success) {
            toast.error(res.message);
          }
          setNextJobsTime(res.data.dateTime);
          if (res.data.noOfJobs === 0) {
            setHasMore(false);
          }
        });
    }   
  }

  const onSortClik = (event) => {
    const sortValue = event.target.value;
    dispatch(userJobActions.onChange('sortType', sortValue));
    setHasMore(true);
    setNextJobsTime(null);
    setIsLoading(true);
    const data = setupGetJobsDataForSort(sortValue, false);
    dispatch(userJobActions.getJobsByFilters(data, false)).then((res) => {
      setIsLoading(false);
      if (!res?.success) {
        toast.error(res.message);
      }
      sortAmplitude(sortValue);
      setNextJobsTime(res.data.dateTime);
      if (res.data.noOfJobs === 0) {
        setHasMore(false);
      }
    });
  };

  const onSelectTitle = (value, titVal) => {
    if (titVal?.target?.checked) {
      Amplitude('Filter job', {
        'job title': `${value.name}`,
      });
    } else {
      Amplitude('Remove job filter', {
        'job title': `${value.name}`,
      });
    }
    dispatch(userJobActions.onSelectTitles(value));
    setHasMore(true);
    onGetJobData(false);
  };
  const onSelectSkill = (value, sklVal) => {
    if (sklVal?.target?.checked) {
      Amplitude('Filter job', {
        skills: `${value.name}`,
      });
    } else {
      Amplitude('Remove job filter', {
        skills: `${value.name}`,
      });
    }
    dispatch(userJobActions.onSelectSkills(value));
    setHasMore(true);
    onGetJobData(false);
  };
  const onSelectIndustry = (value, indVal) => {
    if (indVal?.target?.checked) {
      Amplitude('Filter job', {
        industry: `${value.name}`,
      });
    } else {
      Amplitude('Remove job filter', {
        industry: `${value.name}`,
      });
    }
    dispatch(userJobActions.onSelectIndustries(value));
    setHasMore(true);
    onGetJobData(false);
  };
  const onSelectLocation = (value, locVal) => {
    if (locVal?.target?.checked) {
      Amplitude('Filter job', {
        location: `${value.name}`,
      });
    } else {
      Amplitude('Remove job filter', {
        location: `${value.name}`,
      });
    }
    dispatch(userJobActions.onSelectLocations(value));
    setHasMore(true);
    onGetJobData(false);
  };
  const onClearAllFields = () => {
    dispatch(userJobActions.onClearFilter());
    setHasMore(true);
    onGetJobData(true);
  };
  
  const rolesCount = (totalRolesCount) => {
    if (totalRolesCount === null|| totalRolesCount === 0) {
      return '0 roles';
    } 
    else if (totalRolesCount > 1 || totalRolesCount === 0) {
      return nf.format(totalRolesCount) + ' roles';
    } 
    else {
      return totalRolesCount + ' role';
    }
  };

  const categorySort = (
    <div className="sortMainWrapper">
            <div className="sortRow">
              <div className="jobCategoryContainer">
                <input
                  name="external-exclusive"
                  type="radio"
                  autoComplete="off"
                  id="radio-all"
                  value="All"
                  checked={sortType === 'All'}
                  onClick={onSortClik}
                  onChange={() => {
                    //This is intentional
                  }}
                />
                <label htmlFor="radio-all">
                  All Jobs
                </label>
                <input
                  name="external-exclusive"
                  type="radio"
                  autoComplete="off"
                  id="radio-exclusive"
                  value="Exclusive"
                  checked={sortType === 'Exclusive'}
                  onClick={onSortClik}
                  onChange={() => {
                    //This is intentional
                  }}
                />
                <label htmlFor="radio-exclusive">
                  Exclusive
                </label>
                <input
                  name="external-exclusive"
                  type="radio"
                  autoComplete="off"
                  id="radio-external"
                  value="External"
                  checked={sortType === 'External'}
                  onClick={onSortClik}
                  onChange={() => {
                    //This is intentional
                  }}
                />
                <label htmlFor="radio-external">
                  External
                </label>
              </div>
            </div>
          </div>
  )

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
        <div>
        </div>
        <div className="noOfJobMainWrapper">
          <div className='categorySortMain'>
            {categorySort}
          </div>
          <div className="searchBar"><SearchIcon/><input value={search}
          type="search"
          onChange={(e) => {
            setSearch(e.target.value);
          }} placeholder={width >= breakpoint ? "Search for available roles": "Search roles"} 
          onKeyDown={searchRoles}/></div>
          <div className="filterToggleBtnWrapper">
            <button
              className="filterToggleBtn"
              onClick={() => setShowFilter(true)}
            >
              <FilterAltIcon />
              <span>Filter</span>
            </button>
          </div>
          <div className="totalNoOfRoleText">
            {rolesCount(totalRolesCount)}
          </div>
        </div>

        <div className="jobAndFiltersWrapper">
          <div
            className={`${'filterWrapper'}${
              showFilter ? ' mobileFilterWrapper' : ''
            }`}
          >
            <SearchFilter
              setShowFilter={() => setShowFilter(false)}
              showFilter={showFilter}
              setHasMore={(value) => setHasMore(value)}
              setNextJobsTime={(value) => setNextJobsTime(value)}
              categorySort={categorySort}
            />
          </div>
          <div className="jobSection">
      {width >= breakpoint && <div className="displayAllFilterVal">
        {selectedTitles?.length > 0 && 
            selectedTitles?.map((tit) => (
              <div className="displayItem" key={tit.id}>
                <span>{tit.name}</span>
                <button onClick={() => onSelectTitle(tit, false)}>
                  <img src={MdClose} alt="close" />
                </button>
              </div>
            ))
        }
        {selectedSkills?.length > 0 && 
            selectedSkills?.map((skl) => (
              <div className="displayItem" key={skl.id}>
                <span>{skl.name}</span>
                <button onClick={() => onSelectSkill(skl, false)}>
                  <img src={MdClose} alt="close" />
                </button>
              </div>
            ))
        }
        {selectedIndustries?.length > 0 && 
            selectedIndustries?.map((ind) => (
              <div className="displayItem" key={ind.id}>
                <span>{ind.name}</span>
                <button onClick={() => onSelectIndustry(ind, false)}>
                  <img src={MdClose} alt="close" />
                </button>
              </div>
            ))
        }
        {selectedLocations?.length > 0 && 
            selectedLocations?.map((loc) => (
              <div className="displayItem" key={loc.id}>
                <span>{loc.name}</span>
                <button onClick={() => onSelectLocation(loc, false)}>
                <img src={MdClose} alt="close" />
                </button>
              </div>
            ))
        }
        {selectedSkillIds?.length > 0 ||
        selectedTitleIds?.length > 0 ||
        selectedIndustrieIds?.length > 0 ||
        selectedLocationIds?.length > 0 ? (
          <button className="filterClearTxt" onClick={() => onClearAllFields()}>
            Clear all
          </button>
        ) : null}
      </div>}
      <span>
        </span>
            <InfiniteScroll
              dataLength={allJobs?.length}
              next={getMoreJobs}
              hasMore={hasMore}
              className="infiniteJobBoard"
            >
              <div className="jobsBoardDisplay">
                {!allJobs?.length && "No results found"}
                {allJobs?.length > 0 &&
                  allJobs.map((job) => (
                    <JobBoard key={job.id} job={job} setJobModalShow={setJobModalShow} />
                  ))}
              </div>
            </InfiniteScroll>
          </div>
        </div>
      </ScreenContainer>
      <JobsBoardModal
        show={jobModalShow}
        onHide={() => setJobModalShow(false)}
      />
    </div>
  );
}
export default Home;
