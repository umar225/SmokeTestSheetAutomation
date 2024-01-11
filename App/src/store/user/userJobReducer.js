/* eslint-disable import/no-anonymous-default-export */
import { userJobTypes } from './userJobTypes';
import _ from 'lodash';

const INITIAL_STATE = {
  allSkills: [],
  allIndustries: [],
  allLocations: [],
  allTitles: [],
  sortType: '',
  searchString: '',
  sortOrder: 'Recent',
  selectedSkills: [],
  selectedIndustries: [],
  selectedLocations: [],
  selectedTitles: [],
  selectedSkillIds: [],
  selectedIndustrieIds: [],
  selectedLocationIds: [],
  selectedTitleIds: [],
  allJobs: [],
  totalRolesCount: null,
  lastSearch: {
    selectedSkills: [],
    selectedIndustries: [],
    selectedLocations: [],
    selectedTitles: [],
    selectedSkillIds: [],
    selectedIndustrieIds: [],
    selectedLocationIds: [],
    selectedTitleIds: [],
  },
};

export default (state, action) => {
  if (!state) state = INITIAL_STATE;
  switch (action.type) {
    case userJobTypes.USER_JOB_ONCHANGE:
      return {
        ...state,
        [action.payload.prop]: action.payload.value,
      };
    case userJobTypes.SELECTED_TITLES: {
      const newState = { ...state };
      const beforeSelectedTitles = JSON.parse(
        JSON.stringify(newState.selectedTitles)
      );
      const beforeSelectedTitleIds = JSON.parse(
        JSON.stringify(newState.selectedTitleIds)
      );
      const selectedItem = action.payload;
      const isExist = newState.selectedTitles.find((tit) => {
        return tit.id === selectedItem.id;
      });
      if (isExist) {
        _.remove(newState.selectedTitles, (tit) => {
          return tit.id === selectedItem.id;
        });
        _.remove(newState.selectedTitleIds, (tit) => {
          return tit === selectedItem.id;
        });
      } else {
        newState.selectedTitles.push(selectedItem);
        newState.selectedTitleIds.push(selectedItem.id);
      }
      return {
        ...newState,
        lastSearch: {
          ...newState.lastSearch,
          selectedTitles: beforeSelectedTitles,
          selectedTitleIds: beforeSelectedTitleIds,
          selectedSkills: newState.selectedSkills,
          selectedSkillIds: newState.selectedSkillIds,
          selectedLocations: newState.selectedLocations,
          selectedLocationIds: newState.selectedLocationIds,
          selectedIndustries: newState.selectedIndustries,
          selectedIndustrieIds: newState.selectedIndustrieIds,
        },
      };
    }
    case userJobTypes.SELECTED_SKILLS: {
      const newState = { ...state };
      const beforeSelectedSkills = JSON.parse(
        JSON.stringify(newState.selectedSkills)
      );
      const beforeSelectedSkillIds = JSON.parse(
        JSON.stringify(newState.selectedSkillIds)
      );
      const selectedItem = action.payload;
      const isExist = newState.selectedSkills.find((skl) => {
        return skl.id === selectedItem.id;
      });
      if (isExist) {
        _.remove(newState.selectedSkills, (skl) => {
          return skl.id === selectedItem.id;
        });
        _.remove(newState.selectedSkillIds, (skl) => {
          return skl === selectedItem.id;
        });
      } else {
        newState.selectedSkills.push(selectedItem);
        newState.selectedSkillIds.push(selectedItem.id);
      }
      return {
        ...newState,
        lastSearch: {
          ...newState.lastSearch,
          selectedSkills: beforeSelectedSkills,
          selectedSkillIds: beforeSelectedSkillIds,
          selectedLocations: newState.selectedLocations,
          selectedLocationIds: newState.selectedLocationIds,
          selectedIndustries: newState.selectedIndustries,
          selectedIndustrieIds: newState.selectedIndustrieIds,
          selectedTitles: newState.selectedTitles,
          selectedTitleIds: newState.selectedTitleIds,
        },
      };
    }
    case userJobTypes.SELECTED_INDUSTRIESS: {
      const newState = { ...state };
      const selectedItem = action.payload;
      const beforeSelectedIndustries = JSON.parse(
        JSON.stringify(newState.selectedIndustries)
      );
      const beforeSelectedIndustrieIds = JSON.parse(
        JSON.stringify(newState.selectedIndustrieIds)
      );
      const isExist = newState.selectedIndustries.find((ind) => {
        return ind.id === selectedItem.id;
      });
      if (isExist) {
        _.remove(newState.selectedIndustries, (ind) => {
          return ind.id === selectedItem.id;
        });
        _.remove(newState.selectedIndustrieIds, (ind) => {
          return ind === selectedItem.id;
        });
      } else {
        newState.selectedIndustries.push(selectedItem);
        newState.selectedIndustrieIds.push(selectedItem.id);
      }
      return {
        ...newState,
        lastSearch: {
          ...newState.lastSearch,
          selectedIndustries: beforeSelectedIndustries,
          selectedIndustrieIds: beforeSelectedIndustrieIds,
          selectedLocations: newState.selectedLocations,
          selectedLocationIds: newState.selectedLocationIds,
          selectedSkills: newState.selectedSkills,
          selectedSkillIds: newState.selectedSkillIds,
          selectedTitles: newState.selectedTitles,
          selectedTitleIds: newState.selectedTitleIds,
        },
      };
    }
    case userJobTypes.SELECTED_LOCATIONS: {
      const newState = { ...state };
      const selectedItem = action.payload;
      const beforeSelectedLocations = JSON.parse(
        JSON.stringify(newState.selectedLocations)
      );
      const beforeSelectedLocationIds = JSON.parse(
        JSON.stringify(newState.selectedLocationIds)
      );
      const isExist = newState.selectedLocations.find((loc) => {
        return loc.id === selectedItem.id;
      });
      if (isExist) {
        _.remove(newState.selectedLocations, (loc) => {
          return loc.id === selectedItem.id;
        });
        _.remove(newState.selectedLocationIds, (loc) => {
          return loc === selectedItem.id;
        });
      } else {
        newState.selectedLocations.push(selectedItem);
        newState.selectedLocationIds.push(selectedItem.id);
      }
      return {
        ...newState,
        lastSearch: {
          ...newState.lastSearch,
          selectedLocations: beforeSelectedLocations,
          selectedLocationIds: beforeSelectedLocationIds,
          selectedIndustries: newState.selectedIndustries,
          selectedIndustrieIds: newState.selectedIndustrieIds,
          selectedSkills: newState.selectedSkills,
          selectedSkillIds: newState.selectedSkillIds,
          selectedTitles: newState.selectedTitles,
          selectedTitleIds: newState.selectedTitleIds,
        },
      };
    }
    case userJobTypes.SELECTED_SORTRELEVANTTYPE: {
      const newState = { ...state };
      return {
        ...newState,
        selectedSkills: newState.lastSearch.selectedSkills,
        selectedIndustries: newState.lastSearch.selectedIndustries,
        selectedLocations: newState.lastSearch.selectedLocations,
        selectedSkillIds: newState.lastSearch.selectedSkillIds,
        selectedIndustrieIds: newState.lastSearch.selectedIndustrieIds,
        selectedLocationIds: newState.lastSearch.selectedLocationIds,
        selectedTitles: newState.lastSearch.selectedTitles,
        selectedTitleIds: newState.lastSearch.selectedTitleIds,
      };
    }
    case userJobTypes.SELECTED_SORTRECENTTYPE: {
      const newState = { ...state };
      return {
        ...newState,
        lastSearch: {
          ...newState.lastSearch,
          selectedLocations: newState.selectedLocations,
          selectedLocationIds: newState.selectedLocationIds,
          selectedIndustries: newState.selectedIndustries,
          selectedIndustrieIds: newState.selectedIndustrieIds,
          selectedSkills: newState.selectedSkills,
          selectedSkillIds: newState.selectedSkillIds,
          selectedTitles: newState.selectedTitles,
          selectedTitleIds: newState.selectedTitleIds,
        },
      };
    }
    case userJobTypes.CLEAR_FILTER_VALUE:
      return {
        ...state,
        selectedSkills: [],
        selectedIndustries: [],
        selectedLocations: [],
        selectedSkillIds: [],
        selectedIndustrieIds: [],
        selectedLocationIds: [],
        selectedTitles: [],
        selectedTitleIds: [],
      };
    default:
      return state;
  }
};
