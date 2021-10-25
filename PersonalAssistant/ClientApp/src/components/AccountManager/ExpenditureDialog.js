import React, { Component } from 'react';

import AddOrEditForm from '../AddOrEditForm';
import { HttpMethod } from '../../helper/GeneralConstants';
import { WebApi } from '../../helper/RouterConstants';
import { inputType } from '../../helper/GeneralConstants';
import oidc from '../oidc';

class ExpenditureDialog extends Component {
    constructor(props){
        super(props);
        this.state = {
            values: [null, null, null, null, null, null, null],
            accountOptions: [],
            wayOptions: [],
            typeOptions: []
        };
        this.setValues = this.setValues.bind(this);
    }
    setValues(values){
        this.setState({values: values});
    }
    componentDidMount(){
        oidc(HttpMethod.Get, WebApi.AccountInitializations).then(json => {
            this.setState({ accountOptions: json.map(e=>({label: e.Name, value: e.ID})) });
        });
        oidc(HttpMethod.Get, WebApi.ExpenditureWays).then(json => {
            this.setState({ wayOptions: json.map(e=>({label: e.WayName, value: e.ID})) });
        });
        oidc(HttpMethod.Get, WebApi.ExpenditureTypes).then(json => {
            this.setState({ typeOptions: json.map(e=>({label: e.TypeName, value: e.ID})) });
        });
    }
    render(){
        const {accountOptions, wayOptions, typeOptions} = this.state;
        if(accountOptions.length === 0 || wayOptions.length === 0 || typeOptions.length === 0){
            return null;
        }
        const fieldsInfo = [
            { label: 'EffectiveDate', type: inputType.datetime, required: true },
            { label: 'Account', type: inputType.select, required: true, selectOptions: accountOptions},
            { label: 'Amount', type: inputType.number, required: true },
            { label: 'Fees', type: inputType.number },
            { label: 'Remarks', type: inputType.text },
            { label: 'ExpenditureWay', type: inputType.select, required: true, selectOptions: wayOptions },
            { label: 'ExpenditureType', type: inputType.select, required: true, selectOptions: typeOptions }
        ];
        return <AddOrEditForm title={'Expenditure'} fieldsInfo={fieldsInfo} />;
    }
}

export default ExpenditureDialog;