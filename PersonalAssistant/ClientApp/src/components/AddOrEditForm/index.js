import React from 'react';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';

import CustomInput from './CustomInput';

const AddOrEditForm = (props) => {
    const { title, fieldsInfo, values, setValues } = props;
    let fields = fieldsInfo.map((e, i)=>
        <Grid key={i} item xs={12} sm={6}>
            <CustomInput fieldInfo={e} setValues={setValues} values={values} index={i} fullWidth />
        </Grid>);
    return (
        <Grid container spacing={2}>
            <Grid item xs={12}>
                <Typography variant="h3" gutterBottom>
                    {title}
                </Typography>
            </Grid>
            {fields}
        </Grid>
    );

}

export default AddOrEditForm;