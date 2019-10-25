import { CHANGE_LANGUAGE } from '../actionTypes';
import translation from '../../translation'
import zhtw from '../../translation/zh-tw';

const localStorageKey = 'Language';
const localStorageLanguage = window.localStorage.getItem(localStorageKey);
const initCode = localStorageLanguage? localStorageLanguage: zhtw.code;
const initialState = {
    lang: initCode,
    translation: translation[initCode]
};

export default (state = initialState, action) => {
    switch (action.type) {
        case CHANGE_LANGUAGE: {
            window.localStorage.setItem(localStorageKey, action.payload.lang);
            return {
                lang: action.payload.lang,
                translation: translation[action.payload.lang]
            };
        }
        default: {
            return state;
        }
    }
};
