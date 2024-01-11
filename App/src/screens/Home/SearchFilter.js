import React, { useEffect, useState } from 'react';
import Accordion from 'react-bootstrap/Accordion';
import { userJobActions } from '../../store/user/userJobActions';
import { useDispatch, useSelector } from 'react-redux';
import Form from 'react-bootstrap/Form';
import Loader from '../../components/Loader';
import { toast } from 'react-toastify';
import Amplitude from '../../utils/Amplitude';
import MdClose from '../../utils/images/Vector.svg';
import PropTypes from 'prop-types';

export const SearchFilter = ({
  setShowFilter,
  showFilter,
  setHasMore,
  setNextJobsTime,
  categorySort
}) => {
  const dispatch = useDispatch();

  const [isLoading, setIsLoading] = useState(false);
  const [skillsToShow, setSkillsToShow] = useState(5);
  const [industriesToShow, setIndustriesToShow] = useState(5);
  const [locationsToShow, setLocationsToShow] = useState(5);
  const nf = new Intl.NumberFormat('en-US');
  const [width, setWidth] = useState(window.innerWidth);
  const breakpoint = 600;

  useEffect(() => {
    setIsLoading(true);
    const handleWindowResize = () => setWidth(window.innerWidth);
    window.addEventListener('resize', handleWindowResize);
    dispatch(userJobActions.onGetAllFilterValue()).then((res) => {
      setIsLoading(false);
      if (res?.success && res.data) {
        dispatch(userJobActions.onChange('allSkills', res.data.skills));
        dispatch(userJobActions.onChange('allIndustries', res.data.industries));
        dispatch(userJobActions.onChange('allLocations', res.data.locations));
        dispatch(userJobActions.onChange('allTitles', res.data.titles));
        if (sortType == null || sortType == '') {
          dispatch(userJobActions.onChange('sortType', res.data.sort[1]));
        }
      }
    });
  }, []);

  const checkSelectedTitle = (itemId) => {
    let result = false;
    if (selectedTitles?.length > 0) {
      const isExist = selectedTitles.find((tit) => {
        return tit.id === itemId;
      });
      if (isExist) {
        result = true;
      }
    }
    return result;
  };
  const checkSelectedSkill = (itemId) => {
    let result = false;
    if (selectedSkills?.length > 0) {
      const isExist = selectedSkills.find((skl) => {
        return skl.id === itemId;
      });
      if (isExist) {
        result = true;
      }
    }
    return result;
  };
  const checkSelectedIndusties = (itemId) => {
    let result = false;
    if (selectedIndustries?.length > 0) {
      const isExist = selectedIndustries.find((ind) => {
        return ind.id === itemId;
      });
      if (isExist) {
        result = true;
      }
    }
    return result;
  };
  const checkSelectedLocations = (itemId) => {
    let result = false;
    if (selectedLocations?.length > 0) {
      const isExist = selectedLocations.find((loc) => {
        return loc.id === itemId;
      });
      if (isExist) {
        result = true;
      }
    }
    return result;
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

  const showmore = (all, filterName) => {
    if (filterName == 'skills') setSkillsToShow(all);
    if (filterName == 'industries') setIndustriesToShow(all);
    if (filterName == 'locations') setLocationsToShow(all);
  };

  const {
    allSkills,
    allIndustries,
    allLocations,
    allTitles,
    sortType,
    selectedSkills,
    selectedIndustries,
    selectedLocations,
    selectedTitles,
    selectedSkillIds,
    selectedIndustrieIds,
    selectedLocationIds,
    selectedTitleIds,
    searchString,
  } = useSelector((state) => state.UserJobReducer);
  return (
    <div className="filterWidthWrapper">
      {isLoading && <Loader />}
      {showFilter && (<div className="filterHeader">
        <button className="filterCloseIcon" onClick={() => setShowFilter()}>
          <img src={MdClose} alt="close" />
        </button>
        <span className="filterText">Filter</span>
        <span>
        {selectedSkillIds?.length > 0 ||
        selectedTitleIds?.length > 0 ||
        selectedIndustrieIds?.length > 0 ||
        selectedLocationIds?.length > 0 ? (
          <button className="filterClearTxt" onClick={() => onClearAllFields()}>
            Clear
          </button>
        ) : null}
        </span>
      </div>)}
      <div className="seprateLineSpecific" />
      {width <= breakpoint && (
        <div className="categorySortMobile">
        {categorySort}
      </div>
      )}
      <div className="seprateLineSpecific" />
      {width <= breakpoint && (<div className="displayAllFilterVal">
        {selectedTitles?.length > 0 && (
          <div className="displayCategory">
            {selectedTitles?.map((tit) => (
              <div className="displayItem" key={tit.id}>
                <span>{tit.name}</span>
                <button onClick={() => onSelectTitle(tit, false)}>
                  <img src={MdClose} alt="close"/>
                </button>
              </div>
            ))}
          </div>
        )}
        {selectedSkills?.length > 0 && (
          <div className="displayCategory">
            {selectedSkills?.map((skl) => (
              <div className="displayItem" key={skl.id}>
                <span>{skl.name}</span>
                <button onClick={() => onSelectSkill(skl, false)}>
                  <img src={MdClose} alt="close" />
                </button>
              </div>
            ))}
          </div>
        )}
        {selectedIndustries?.length > 0 && (
          <div className="displayCategory">
            {selectedIndustries?.map((ind) => (
              <div className="displayItem" key={ind.id}>
                <span>{ind.name}</span>
                <button onClick={() => onSelectIndustry(ind, false)}>
                  <img src={MdClose} alt="close" />
                </button>
              </div>
            ))}
          </div>
        )}
        {selectedLocations?.length > 0 && (
          <div className="displayCategory">
            {selectedLocations?.map((loc) => (
              <div className="displayItem" key={loc.id}>
                <span>{loc.name}</span>
                <button onClick={() => onSelectLocation(loc, false)}>
                <img src={MdClose} alt="close" />
                </button>
              </div>
            ))}
          </div>
        )}
      </div>)}
      {selectedSkillIds?.length > 0 ||
      selectedIndustrieIds?.length > 0 ||
      selectedTitleIds?.length > 0 ||
      selectedLocationIds?.length > 0 ? (
        <div className="seprateLineSpecific" />
      ) : null}
      <div className="filterAccordionWrapper">
        <Accordion defaultActiveKey={['0']} alwaysOpen>
          <Accordion.Item eventKey="0">
            <Accordion.Header>Job Title</Accordion.Header>
            <Accordion.Body>
              <Form.Group>
                {allTitles.slice(0, 5)?.map((title) => (
                  <div className="filterOptionVal" key={title.id}>
                    <Form.Check
                      className="checkBoxMargin"
                      type={'checkbox'}
                      label={title.name}
                      checked={checkSelectedTitle(title.id)}
                      onChange={(e) => onSelectTitle(title, e)}
                    />
                    <span>{`${'('}${nf.format(title.noOfJob)}${')'}`}</span>
                  </div>
                ))}
              </Form.Group>
            </Accordion.Body>
          </Accordion.Item>
          <span className="seprateLine"></span>
          <Accordion.Item eventKey="1">
            <Accordion.Header>Skills</Accordion.Header>
            <Accordion.Body>
              <Form.Group>
                {allSkills.slice(0, skillsToShow)?.map((skill) => (
                  <div className="filterOptionVal" key={skill.id}>
                    <Form.Check
                      className="checkBoxMargin"
                      type={'checkbox'}
                      label={skill.name}
                      checked={checkSelectedSkill(skill.id)}
                      onChange={(e) => onSelectSkill(skill, e)}
                    />
                    <span>{`${'('}${nf.format(skill.noOfJob)}${')'}`}</span>
                  </div>
                ))}
                {skillsToShow === 5 ? (
                  <div className="showMoreFilters">
                    <button
                      onClick={() => showmore(allSkills.length, 'skills')}
                    >
                      <b>more</b>
                    </button>
                  </div>
                ) : (
                  <div className="showMoreFilters">
                    <button onClick={() => setSkillsToShow(5)}>
                      <b>less</b>
                    </button>
                  </div>
                )}
              </Form.Group>
            </Accordion.Body>
          </Accordion.Item>
          <span className="seprateLine"></span>
          <Accordion.Item eventKey="2">
            <Accordion.Header>Industry</Accordion.Header>
            <Accordion.Body>
              <Form.Group>
                {allIndustries
                  .slice(0, industriesToShow)
                  ?.map((inds) => (
                    <div className="filterOptionVal" key={inds.id}>
                      <Form.Check
                        className="checkBoxMargin"
                        type={'checkbox'}
                        label={inds.name}
                        checked={checkSelectedIndusties(inds.id)}
                        onChange={(e) => onSelectIndustry(inds, e)}
                      />
                      <span>{`${'('}${nf.format(inds.noOfJob)}${')'}`}</span>
                    </div>
                  ))}
                {industriesToShow === 5 ? (
                  <div className="showMoreFilters">
                    <button
                      onClick={() =>
                        showmore(allIndustries.length, 'industries')
                      }
                    >
                      <b>more</b>
                    </button>
                  </div>
                ) : (
                  <div className="showMoreFilters">
                    <button onClick={() => setIndustriesToShow(5)}>
                      <b>less</b>
                    </button>
                  </div>
                )}
              </Form.Group>
            </Accordion.Body>
          </Accordion.Item>
          <span className="seprateLine"></span>
          <Accordion.Item eventKey="3">
            <Accordion.Header>Location</Accordion.Header>
            <Accordion.Body>
              <Form.Group>
                {allLocations.slice(0, locationsToShow)?.map((loca) => (
                  <div className="filterOptionVal" key={loca.id}>
                    <Form.Check
                      className="checkBoxMargin"
                      type={'checkbox'}
                      label={loca.name}
                      checked={checkSelectedLocations(loca.id)}
                      onChange={(e) => onSelectLocation(loca, e)}
                    />
                    <span>{`${'('}${nf.format(loca.noOfJob)}${')'}`}</span>
                  </div>
                ))}
                {locationsToShow === 5 ? (
                  <div className="showMoreFilters">
                    <button
                      onClick={() => showmore(allLocations.length, 'locations')}
                    >
                      <b>more</b>
                    </button>
                  </div>
                ) : (
                  <div className="showMoreFilters">
                    <button onClick={() => setLocationsToShow(5)}>
                      <b>less</b>
                    </button>
                  </div>
                )}
              </Form.Group>
            </Accordion.Body>
          </Accordion.Item>
        </Accordion>
      </div>
    </div>
  );
};

SearchFilter.propTypes = {
  setShowFilter: PropTypes.func,
  showFilter: PropTypes.bool,
  setHasMore: PropTypes.func,
  setNextJobsTime: PropTypes.func,
  categorySort: PropTypes.node,
}