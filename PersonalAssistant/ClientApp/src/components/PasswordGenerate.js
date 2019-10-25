import React, { useState } from 'react';
import { connect } from "react-redux";
import { Button, Grid, Checkbox, FormControlLabel, TextField, IconButton } from '@material-ui/core';
import { Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle } from '@material-ui/core';
import SettingsIcon from '@material-ui/icons/Settings';
import { defaultSpecialCharacters, LOWERCASELETTERS, UPPERCASELETTERS, NUMBERS, SPECIALCHARACTERS, PasswordGenerator } from '../helper/PasswordGenerator';


const PasswordGenerate = props => {
    const [requirement, setRequirement] = useState({
        includeNumbers: true,
        includeLowercaseLetters: true,
        includeUppercaseLetters: true,
        includeSpecialCharacters: true
    });
    const [specialCharacters, setSpecialCharacters] = useState(defaultSpecialCharacters);
    const [openDialog, setOpenDialog] = React.useState(false);
    const [password, setPassword] = React.useState('');
    return (
        <Grid container direction='column' justify='flex-start' alignItems='flex-start'>
            <Grid item>
                <FormControlLabel
                    control={<Checkbox checked={requirement.includeLowercaseLetters} onChange={(e) => setRequirement({ ...requirement, includeLowercaseLetters: e.target.checked })} />}
                    label={props.translate.includeLowercaseLetters + ': a~z'}
                />
            </Grid>
            <Grid item>
                <FormControlLabel
                    control={<Checkbox checked={requirement.includeUppercaseLetters} onChange={(e) => setRequirement({ ...requirement, includeUppercaseLetters: e.target.checked })} />}
                    label={props.translate.includeUppercaseLetters + ': A~Z'}
                />
            </Grid>
            <Grid item>
                <FormControlLabel
                    control={<Checkbox checked={requirement.includeNumbers} onChange={(e) => setRequirement({ ...requirement, includeNumbers: e.target.checked })} />}
                    label={props.translate.includeNumbers + ': 0~9'}
                />
            </Grid>
            <Grid item>
                <FormControlLabel
                    control={<Checkbox checked={requirement.includeSpecialCharacters} onChange={(e) => setRequirement({ ...requirement, includeSpecialCharacters: e.target.checked })} />}
                    label={props.translate.includeSpecialCharacters + ': ' + specialCharacters}
                />
                <IconButton edge='start' color='inherit' onClick={() => { setOpenDialog(true); }}>
                    <SettingsIcon />
                </IconButton>
            </Grid>
            <Button variant='outlined' fullWidth onClick={() => {
                const generator = new PasswordGenerator(specialCharacters);
                setPassword(generator.generatePassword(0
                    | (requirement.includeLowercaseLetters ? LOWERCASELETTERS : 0)
                    | (requirement.includeUppercaseLetters ? UPPERCASELETTERS : 0)
                    | (requirement.includeNumbers ? NUMBERS : 0)
                    | (requirement.includeSpecialCharacters ? SPECIALCHARACTERS : 0), 12));
            }}>{props.translate.generatePassword}</Button>
            <TextField
                disabled fullWidth
                label={props.translate.password}
                value={password}
                margin="normal"
                variant="outlined"
            />
            <Dialog open={openDialog} onClose={() => { setOpenDialog(false); }} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">{props.translate.setSpecialCharacters}</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        {props.translate.modifySpecialCharacters}
                    </DialogContentText>
                    <TextField autoFocus margin="dense" value={specialCharacters} fullWidth onChange={(e) => setSpecialCharacters(e.target.value)} />
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => { setOpenDialog(false); }}>
                        {props.translate.cancel}
                    </Button>
                    <Button onClick={() => { setOpenDialog(false); }}>
                        {props.translate.confirm}
                    </Button>
                </DialogActions>
            </Dialog>
        </Grid>
    );
}

const mapStateToProps = store => ({
    translate: {
        generatePassword: store.lang.translation.passwordManager.generatePassword,
        password: store.lang.translation.passwordManager.password,
        setSpecialCharacters: store.lang.translation.passwordManager.setSpecialCharacters,
        modifySpecialCharacters: store.lang.translation.passwordManager.modifySpecialCharacters,
        includeLowercaseLetters: store.lang.translation.passwordManager.includeLowercaseLetters,
        includeUppercaseLetters: store.lang.translation.passwordManager.includeUppercaseLetters,
        includeNumbers: store.lang.translation.passwordManager.includeNumbers,
        includeSpecialCharacters: store.lang.translation.passwordManager.includeSpecialCharacters,
        cancel: store.lang.translation.passwordManager.cancel,
        confirm: store.lang.translation.passwordManager.confirm,
    },
    dark: store.theme
});
export default connect(mapStateToProps)(PasswordGenerate);