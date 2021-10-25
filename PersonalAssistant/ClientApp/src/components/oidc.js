import authService from './api-authorization/AuthorizeService';
import { HttpMethod, HttpHeader, ApplicationJson, CharsetUTF8 } from '../helper/GeneralConstants';

async function oidc(method, controllerUrl, data) {
    const token = await authService.getAccessToken();
    let headers = {};
    if (token) {
        headers[HttpHeader.Authorization] = `Bearer ${token}`;
    }
    let jsonUTF8 = `${ApplicationJson};${CharsetUTF8}`;
    let url = controllerUrl;
    switch (method) {
        case HttpMethod.Post:
            headers[HttpHeader.Accept] = jsonUTF8;
            headers[HttpHeader.ContentType] = jsonUTF8;
            break;
        case HttpMethod.Put:
            headers[HttpHeader.ContentType] = jsonUTF8;
            url = `${url}/${data.ID}`;
            break;
        case HttpMethod.Delete:
            url = `${url}/${data.ID}`;
            break;
        case HttpMethod.Get:
            if(data){
                url = `${url}/${data.ID}`;
            }
        default:
            break;
    };
    try{
        let response = await fetch(url, {
            method: method,
            body: !data ? undefined : JSON.stringify(data),
            headers: headers
        });
        let contentType = response.headers.get(HttpHeader.ContentType);
        if (contentType && contentType.includes(ApplicationJson)) {
            return response.json();
        }
        throw new TypeError(`${HttpMethod.Get} ${url} ${HttpHeader.ContentType}: ${contentType}`);
    }catch(error){
        console.error(error);
    }   
}

export default oidc;