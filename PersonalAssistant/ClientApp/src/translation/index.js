import enus from './en-us';
import zhtw from './zh-tw';

function Init() {
    let obj = {};
    obj[enus.code] = enus;
    obj[zhtw.code] = zhtw;
    return obj;
}

export default Init();
