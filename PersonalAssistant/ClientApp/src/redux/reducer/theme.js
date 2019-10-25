import { CHANGE_THEME } from '../actionTypes';

//true = dark, false = light
const localStorageKey = 'Theme';
const localStorageTheme = window.localStorage.getItem(localStorageKey);
const initialState = localStorageTheme? (localStorageTheme === 'true'): true;

export default (state = initialState, action) => {
    switch (action.type) {
        case CHANGE_THEME: {
            window.localStorage.setItem(localStorageKey, (!state).toString());
            return !state;
        }
        default: {
            return state;
        }
    }
};
