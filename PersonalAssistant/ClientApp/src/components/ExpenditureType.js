import React, { Component } from 'react';
import { connect } from "react-redux";
import MaterialTable from 'material-table';
import authService from './api-authorization/AuthorizeService'
import CircularProgress from '@material-ui/core/CircularProgress';

import { HttpMethod, HttpHeader, ApplicationJson, CharsetUTF8 } from '../helper/GeneralConstants'
import { WebApi } from '../helper/RouterConstants'

class ExpenditureType extends Component {
  static displayName = ExpenditureType.name;

  constructor(props) {
    super(props);
    this.state = {
      loading: true,
      columns: [],
      data: []
    };
  }
  async oidc(method, data) {
    const token = await authService.getAccessToken();

    let headers = {};
    if (token) {
      headers[HttpHeader.Authorization] = `Bearer ${token}`;
    }
    let jsonUTF8 = `${ApplicationJson};${CharsetUTF8}`;
    let url = WebApi.ExpenditureTypes;
    switch (method) {
      case HttpMethod.Get:
        break;
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
      default:
        break;
    };
    return fetch(url, {
      method: method,
      body: !data ? undefined : JSON.stringify(data),
      headers: headers
    });
  }
  componentDidMount() {
    this.oidc(HttpMethod.Get).then(response => {
      let contentType = response.headers.get(HttpHeader.ContentType);
      if (contentType && contentType.includes(ApplicationJson)) {
        return response.json();
      }
      throw new TypeError(`${HttpMethod.Get} ${WebApi.ExpenditureTypes} ${HttpHeader.ContentType}: ${contentType}`);
    }).then(json => {
      this.setState({ columns: json.columns, data: json.data, loading: false });
    }).catch(error => console.error(error));
  }

  render() {
    if (this.state.loading) {
      return <CircularProgress />
    }
    return (
      <MaterialTable
        title={this.props.translate.expenditureType}
        columns={this.state.columns}
        data={this.state.data}
        editable={{
          onRowAdd: newData => {
            return this.oidc(HttpMethod.Post, { ...newData, OwnerID: null, ID: null }).then(response => {
              if (!response.ok) {
                throw new Error(`${HttpMethod.Post} ${WebApi.ExpenditureTypes} ${response.statusText}`);
              }
              return response.json();
            }).then(json => {
              this.setState(prevState => {
                const data = [...prevState.data];
                data.push(json);
                return { ...prevState, data };
              });
            }).catch(error => console.error(error));
          },
          onRowUpdate: (newData, oldData) => {
            return this.oidc(HttpMethod.Put, newData).then(response => {
              if (response.ok && oldData) {
                this.setState(prevState => {
                  const data = [...prevState.data];
                  data[data.indexOf(oldData)] = newData;
                  return { ...prevState, data };
                });
              } else {
                throw new Error(`${HttpMethod.Put} ${WebApi.ExpenditureTypes} ${response.statusText}`);
              }
            }).catch(error => console.error(error));
          },
          onRowDelete: oldData => {
            return this.oidc(HttpMethod.Delete, { ...oldData, OwnerID: null }).then(response => {
              if (!response.ok) {
                throw new Error(`${HttpMethod.Put} ${WebApi.ExpenditureTypes} ${response.statusText}`);
              }
              this.setState(prevState => {
                const data = [...prevState.data];
                data.splice(data.indexOf(oldData), 1);
                return { ...prevState, data };
              });
            }).catch(error => console.error(error));
          },
        }}
      />
    );
  }
}

const mapStateToProps = store => ({
  translate: {
    expenditureType: store.lang.translation.accountManager.expenditureType,
  }
});
export default connect(mapStateToProps)(ExpenditureType);