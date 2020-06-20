import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService';

const PrettyPrintJson = React.memo(({data}) => (<div><pre>{
    JSON.stringify(data, null, 2) }</pre></div>));

export default class TempForTestApi extends Component {
  static displayName = TempForTestApi.name;

  constructor(props) {
    super(props);
    this.state = { data: [], loading: true };
  }

  componentDidMount() {
    this.getData();
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : <PrettyPrintJson data={this.state.data} />;

    return (
      <div>
        {contents}
      </div>
    );
  }

  async getData() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/GetLatestStockValue', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ data: data, loading: false });
  }
}
