import React, { useState } from 'react';
import { connect } from "react-redux";
import { Button, Grid, Checkbox, FormControlLabel, TextField, IconButton, Slider, Typography, InputAdornment } from '@material-ui/core';
import { Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle } from '@material-ui/core';
import { Snackbar, SnackbarContent } from '@material-ui/core';
import SettingsIcon from '@material-ui/icons/Settings';
import FileCopyIcon from '@material-ui/icons/FileCopy';
import CheckCircleIcon from '@material-ui/icons/CheckCircle';
import ErrorIcon from '@material-ui/icons/Error';
import CloseIcon from '@material-ui/icons/Close';
import { green } from '@material-ui/core/colors';
import { makeStyles } from '@material-ui/core/styles';
import { defaultSpecialCharacters, LOWERCASELETTERS, UPPERCASELETTERS, NUMBERS, SPECIALCHARACTERS, PasswordGenerator } from '../helper/PasswordGenerator';

const variantIcon = {
  success: CheckCircleIcon,
  error: ErrorIcon
};

const useStyles1 = makeStyles(theme => ({
  success: {
    backgroundColor: green[600],
  },
  error: {
    backgroundColor: theme.palette.error.dark,
  },
  icon: {
    fontSize: 20,
  },
  iconVariant: {
    opacity: 0.9,
    marginRight: theme.spacing(1),
  },
  message: {
    display: 'flex',
    alignItems: 'center',
  },
}));

function MySnackbarContentWrapper(props) {
  const classes = useStyles1();
  const { className, message, onClose, variant, ...other } = props;
  const Icon = variantIcon[variant];

  return (
    <SnackbarContent
      className={classes[variant] + ' ' + className}
      aria-describedby="client-snackbar"
      message={
        <span id="client-snackbar" className={classes.message}>
          <Icon className={classes.icon + ' ' + classes.iconVariant} />
          {message}
        </span>
      }
      action={[
        <IconButton key="close" aria-label="close" color="inherit" onClick={onClose}>
          <CloseIcon className={classes.icon} />
        </IconButton>,
      ]}
      {...other}
    />
  );
}

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
  const [passwordLength, setPasswordLength] = React.useState(12);
  const [snackbarState, setSnackbarState] = React.useState({open:false, variant:'', message: '' });

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
        <IconButton edge='start' color='inherit' onClick={() => setOpenDialog(true)}>
          <SettingsIcon />
        </IconButton>
      </Grid>
      <Typography gutterBottom>
        Length
      </Typography>
      <Slider marks
        color='secondary'
        valueLabelDisplay='auto'
        valueLabelDisplay='on'
        defaultValue={passwordLength}
        onChange={(e, value) => setPasswordLength(value)}
        step={1} min={4} max={34} />
      <Button variant='outlined' fullWidth onClick={() => {
        const generator = new PasswordGenerator(specialCharacters);
        setPassword(generator.generatePassword(0
          | (requirement.includeLowercaseLetters ? LOWERCASELETTERS : 0)
          | (requirement.includeUppercaseLetters ? UPPERCASELETTERS : 0)
          | (requirement.includeNumbers ? NUMBERS : 0)
          | (requirement.includeSpecialCharacters ? SPECIALCHARACTERS : 0), passwordLength));
      }}>{props.translate.generatePassword}</Button>
      <TextField
        disabled fullWidth
        label={props.translate.password}
        value={password}
        margin="normal"
        variant="outlined"
        InputProps={{
          endAdornment: (
            <InputAdornment position="end">
              <IconButton edge='start' color='inherit' onClick={() => {
                navigator.clipboard.writeText(password).then(() => {
                  setSnackbarState({...snackbarState, open:true, variant:'success', message: props.translate.copySuccessful });
                }, (err) => {
                  setSnackbarState({...snackbarState, open:true, variant:'error', message: props.translate.copyError + err });
                });
               }}>
                <FileCopyIcon />
              </IconButton>
            </InputAdornment>
          ),
        }}
      />
      <Dialog open={openDialog} onClose={() => setOpenDialog(false)} aria-labelledby="form-dialog-title">
        <DialogTitle id="form-dialog-title">{props.translate.setSpecialCharacters}</DialogTitle>
        <DialogContent>
          <DialogContentText>
            {props.translate.modifySpecialCharacters}
          </DialogContentText>
          <TextField autoFocus margin="dense" value={specialCharacters} fullWidth onChange={(e) => setSpecialCharacters(e.target.value)} />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpenDialog(false)}>
            {props.translate.cancel}
          </Button>
          <Button onClick={() => setOpenDialog(false)}>
            {props.translate.confirm}
          </Button>
        </DialogActions>
      </Dialog>
      <Snackbar
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'center',
        }}
        open={snackbarState.open}
        autoHideDuration={3000}
        onClose={() => setSnackbarState({...snackbarState, open:false})}
      >
        <MySnackbarContentWrapper
          onClose={() => setSnackbarState({...snackbarState, open:false})}
          variant={snackbarState.variant}
          message={snackbarState.message}
        />
      </Snackbar>
    </Grid>
  );
}

const mapStateToProps = store => ({
  translate: {
    generatePassword: store.lang.translation.generatePassword.generatePassword,
    password: store.lang.translation.generatePassword.password,
    setSpecialCharacters: store.lang.translation.generatePassword.setSpecialCharacters,
    modifySpecialCharacters: store.lang.translation.generatePassword.modifySpecialCharacters,
    includeLowercaseLetters: store.lang.translation.generatePassword.includeLowercaseLetters,
    includeUppercaseLetters: store.lang.translation.generatePassword.includeUppercaseLetters,
    includeNumbers: store.lang.translation.generatePassword.includeNumbers,
    includeSpecialCharacters: store.lang.translation.generatePassword.includeSpecialCharacters,
    copySuccessful: store.lang.translation.generatePassword.copySuccessful,
    copyError: store.lang.translation.generatePassword.copyError,
    cancel: store.lang.translation.generatePassword.cancel,
    confirm: store.lang.translation.generatePassword.confirm,
  }
});
export default connect(mapStateToProps)(PasswordGenerate);