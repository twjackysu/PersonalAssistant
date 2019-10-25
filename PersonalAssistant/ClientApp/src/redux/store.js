import { createStore, combineReducers } from "redux";
import lang from './reducer/lang';
import theme from './reducer/theme';
import showMenu from './reducer/showMenu';
export default createStore(combineReducers({ lang, theme, showMenu }));