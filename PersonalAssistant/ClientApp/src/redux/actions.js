import { CHANGE_LANGUAGE, CHANGE_THEME, OPEN_MENU_LIST, CLOSE_MENU_LIST } from "./actionTypes";

export const change_language = lang => ({
    type: CHANGE_LANGUAGE,
    payload: { lang }
});

export const change_theme = () => ({
    type: CHANGE_THEME
});

export const switch_menu_list = (open) => {
    if (open) {
        return { type: OPEN_MENU_LIST };
    }
    else {
        return { type: CLOSE_MENU_LIST };
    }
};
