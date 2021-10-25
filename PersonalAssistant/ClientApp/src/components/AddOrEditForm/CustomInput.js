import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import MomentUtils from '@date-io/moment';
import {
    KeyboardDateTimePicker,
    MuiPickersUtilsProvider
} from '@material-ui/pickers';
import MenuItem from '@material-ui/core/MenuItem';

import { inputType } from '../../helper/GeneralConstants';

const useStyles = makeStyles({

});

const CustomInput = (props) => {
    const { fieldInfo, setValues, values, index, variant, fullWidth } = props;
    const classes = useStyles();
    let myFullWidth = fullWidth? fullWidth : false;
    let myVariant = variant? variant: 'outlined';
    const handleTextChange = (e) => {
        let cloneValues = [...values];
        cloneValues.splice(index, 1, e.target.value);
        setValues(cloneValues);
    };
    const handleDateTimeChange = (date) => {
        let cloneValues = [...values];
        cloneValues.splice(index, 1, date);
        setValues(cloneValues);
    };
    switch(fieldInfo.type){
        case inputType.datetime:
            return (
                <MuiPickersUtilsProvider utils={MomentUtils}>
                    <KeyboardDateTimePicker
                        disableToolbar
                        label={fieldInfo.label}
                        variant={myVariant}
                        fullWidth={myFullWidth}
                        required={fieldInfo.required}
                        format={fieldInfo.format? fieldInfo.format: 'YYYY/MM/DD HH:mm:ss'}
                        value={values[index]}
                        onChange={handleDateTimeChange} />
                </MuiPickersUtilsProvider>
            );
        case inputType.select:
            return (
                <TextField select
                    label={fieldInfo.label}
                    variant={myVariant}
                    fullWidth={myFullWidth}
                    required={fieldInfo.required}
                    value={values[index]}
                    onChange={handleTextChange}
                    helperText={fieldInfo.helperText}>
                    {fieldInfo.required? null : 
                    <MenuItem value={null}>
                        {'No selected'}
                    </MenuItem>}
                    {fieldInfo.selectOptions.map((option) => (
                        <MenuItem key={option.value} value={option.value}>
                            {option.label}
                        </MenuItem>
                    ))}
                </TextField>
            );
        case inputType.text:
            return (
                <TextField
                    label={fieldInfo.label}
                    variant={myVariant}
                    fullWidth={myFullWidth}
                    required={fieldInfo.required}
                    value={values[index]}
                    onChange={handleTextChange} />
            );
        case inputType.number:
            return (
                <TextField
                    type={inputType.number}
                    label={fieldInfo.label}
                    variant={myVariant}
                    fullWidth={myFullWidth}
                    required={fieldInfo.required}
                    value={values[index]}
                    onChange={handleTextChange} />
            );
        default: return null;
    }

}

export default CustomInput;