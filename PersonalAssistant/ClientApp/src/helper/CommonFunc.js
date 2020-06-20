import authService from '../components/api-authorization/AuthorizeService';
export async function populateData(url) {
    const token = await authService.getAccessToken();
    const response = await fetch(url, {
        headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    return data;
};