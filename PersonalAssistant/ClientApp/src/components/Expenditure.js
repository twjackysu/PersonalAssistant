import React, { Component } from 'react';
import { connect } from "react-redux";
import { withStyles } from '@material-ui/core/styles';
import MaterialTable from 'material-table';
import authService from './api-authorization/AuthorizeService'
import CircularProgress from '@material-ui/core/CircularProgress';
import Paper from '@material-ui/core/Paper';

import { HttpMethod, HttpHeader, ApplicationJson, CharsetUTF8 } from '../helper/GeneralConstants';
import { WebApi, MTApi } from '../helper/RouterConstants';

const styles = {
  root: {
    width: '100vw',
  }
};

class Expenditure extends Component {
  static displayName = Expenditure.name;

  constructor(props) {
    super(props);
    this.state = {
      loading: true,
      title: '',
      columns: [],
      data: []
    };
  }
  componentDidMount() {
    
    this.setState({ data: json.data, columns: json.columns, title: json.title, loading: false });
  }

  render() {
    if (this.state.loading) {
      return <CircularProgress />
    }
    const { controller, translate, classes } = this.props;
    return (
      <MaterialTable
        components={{
          Container: props => (
            <Paper elevation={0} className={classes.root} {...props} />
          ),
        }}
        title={translate[this.state.title]}
        columns={this.state.columns.map(x => ({...x, title: translate[x.field], field: x.field}))}
        data={this.state.data}
        options={{
          headerStyle: {
            textAlign: 'center'
          },
          cellStyle: {
            textAlign: 'center'
          }
        }}
        localization={{
          header: {
            actions: translate.actions
          },
          body: {
            emptyDataSourceMessage: translate.noRecordsToDisplay,
            addTooltip: translate.add,
            deleteTooltip: translate.delete,
            editTooltip: translate.edit,
            editRow: {
              deleteText: translate.areYouSureDeleteThisRow,
              cancelTooltip: translate.cancel,
              saveTooltip: translate.save
            }
          }
        }}
        editable={{
          onRowAdd: newData => {
            console.log(newData);
            return this.oidc(HttpMethod.Post, { ...newData, OwnerID: null, ID: null }).then(response => {
              if (!response.ok) {
                throw new Error(`${HttpMethod.Post} ${WebApi[controller]} ${response.statusText}`);
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
                throw new Error(`${HttpMethod.Put} ${WebApi[controller]} ${response.statusText}`);
              }
            }).catch(error => console.error(error));
          },
          onRowDelete: oldData => {
            return this.oidc(HttpMethod.Delete, { ...oldData, OwnerID: null }).then(response => {
              if (!response.ok) {
                throw new Error(`${HttpMethod.Put} ${WebApi[controller]} ${response.statusText}`);
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
    ...store.lang.translation.materialTable,
    ...store.lang.translation.accountManager,
  }
});
export default connect(mapStateToProps)(withStyles(styles)(Expenditure));