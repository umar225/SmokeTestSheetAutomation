import { accountTypes } from './accountTypes';

const INITIAL_STATE = {
  errorMessage: '',
};

export default (state, action) => {
  if (!state) state = INITIAL_STATE;
  if (action.type === accountTypes.ACCOUNT_ONCHANGE) {
    return {
      ...state,
      [action.payload.prop]: action.payload.value,
    };
  }
  else {
    return state;
  }
};
