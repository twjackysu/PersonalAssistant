import { OPEN_MENU_LIST, CLOSE_MENU_LIST } from '../actionTypes';

//true = open, false = close
const initialState = false;

export default (state = initialState, action) => {
    switch (action.type) {
        case OPEN_MENU_LIST: {
            return true;
        }
        case CLOSE_MENU_LIST: {
            return false;
        }
        default: {
            return state;
        }
    }
};
