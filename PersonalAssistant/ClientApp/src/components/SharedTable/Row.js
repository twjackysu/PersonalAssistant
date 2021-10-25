
import React, { Component } from 'react';
import { withStyles } from '@material-ui/core/styles';
import TableRow from '@material-ui/core/TableRow';
import TableCell from '@material-ui/core/TableCell';
import Chip from '@material-ui/core/Chip';
import Box from '@material-ui/core/Box';
import Collapse from '@material-ui/core/Collapse';
import IconButton from '@material-ui/core/IconButton';
import Grid from '@material-ui/core/Grid';
import Link from '@material-ui/core/Link';
import Checkbox from '@material-ui/core/Checkbox';
import CheckBoxOutlineBlankIcon from '@material-ui/icons/CheckBoxOutlineBlank';
import CheckBoxIcon from '@material-ui/icons/CheckBox';
import Tooltip from '@material-ui/core/Tooltip';
import KeyboardArrowDownIcon from '@material-ui/icons/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp';
import HighchartsReact from 'highcharts-react-official';
import Highcharts from 'highcharts';
import moment from 'moment';

import BodyCell from './BodyCell';

const styles = {
    expandCell: {
        width: '1em'
    },
    container: {
        height: '70px'
    },
    pointer: {
        cursor: 'pointer'
    },
    checkbox: {
        width: '1em',
        height: '1em',
        cursor: 'default',
        padding: '0px'
    },
    collapseCell: {
        paddingBottom: 0,
        paddingTop: 0
    }
};
class Row extends Component {
    constructor(props) {
        super(props);
        this.state = {
            openCollapse: false
        };
        this.handleClick = this.handleClick.bind(this);
    }
    handleCollapse(isOpen) {
        this.setState({ openCollapse: isOpen });
    }
    handleClick(e) {
        this.setState((prevState) => ({ openCollapse: !prevState.openCollapse }));
    }
    field(data, fieldNames){
        let fieldArray = fieldNames.split('.');
        let obj = data[fieldArray[0]];
        fieldArray.forEach((fieldName)=>{
            obj = obj[fieldName];
        });
        return obj;
    }
    render() {
        const { openCollapse } = this.state;
        const { data, columnsInfo, title, collapseRow, bodyRowClass, classes } = this.props;
        const cells = columnsInfo.map((columnInfo, index) => {
            let content = '--';
            if(this.field(data, columnInfo.fieldName)){
                switch (columnInfo.type) {
                    case 'custom':
                        content = columnInfo.customCallBack(data);
                        break;
                    case 'chip':
                        content = <Chip size='small' className={columnInfo.chipClassCallBack? columnInfo.chipClassCallBack(data): undefined} label={this.field(data, columnInfo.fieldName)} />
                        break;
                    case 'date':
                        content = moment.utc(this.field(data, columnInfo.fieldName)).local().format('YYYY-MM-DD HH:mm:ss');
                        break;
                    case 'link':
                        let url = columnInfo.href.template;
                        for (let customSign in columnInfo.href.customSignAndFieldMapping) {
                            let fieldName = columnInfo.href.customSignAndFieldMapping[customSign];
                            url = url.replace(customSign, this.field(data, fieldName));
                        }
                        content = (
                            <Link href={url} target='_blank'>
                                {this.field(data, columnInfo.fieldName)}
                            </Link>
                        );
                        break;
                    case 'boolean':
                        content = (
                            <Checkbox
                                className={classes.checkbox}
                                icon={<CheckBoxOutlineBlankIcon />}
                                checkedIcon={<CheckBoxIcon />}
                                disableRipple={true}
                                checked={this.field(data, columnInfo.fieldName)}
                                color='primary' />
                        )
                        break;
                    case 'number':
                        content = <span>{this.field(data, columnInfo.fieldName).toLocaleString()}</span>;
                        break;
                    case 'trending':
                        let xCategories = Object.keys(this.field(data, columnInfo.fieldName)).sort();
                        let needSetMin = xCategories.some(month => this.field(data, columnInfo.fieldName)[month] < 10);
                        let yAxis = {
                            title: {
                                text: ''
                            },
                            labels: {
                                enabled: false
                            },
                            min: needSetMin? 0: undefined,
                            startOnTick: false
                        };
                        let options = {
                            chart: {
                                height: 70,
                                width: 100,
                                type: 'area'
                            },
                            title: {
                                text: ''
                            },
                            xAxis: {
                                categories: xCategories,
                                labels: {
                                    enabled: false
                                },
                            },
                            legend: {
                                enabled: false
                            },
                            yAxis: yAxis,
                            credits: {
                                enabled: false
                            },
                            tooltip: {
                                enabled: false
                            },
                            plotOptions: {
                                area: {
                                    marker: {
                                        enabled: false,
                                        symbol: 'circle',
                                        radius: 2,
                                        states: {
                                            hover: {
                                                enabled: true
                                            }
                                        }
                                    },
                                    threshold: null
                                }
                            },
                            series: [{
                                data: xCategories.map(m => this.field(data, columnInfo.fieldName)[m])
                            }]
                        };
                        content = (
                            <Grid container justify='center' alignItems='center'>
                                <Grid item>
                                    <HighchartsReact highcharts={Highcharts} options={options} containerProps={{ className: classes.container }} />
                                </Grid>
                            </Grid>
                        );
                        break;
                    default:
                        content = <span>{this.field(data, columnInfo.fieldName)}</span>;
                        break;
                }
            }
            
            let className = '';
            if (columnInfo.onClick) {
                className += classes.pointer;
            }
            if (columnInfo.className) {
                className += ` ${columnInfo.className}`;
            }
            if (columnInfo.tooltipFieldName) {
                content = (
                    <Tooltip title={this.field(data, columnInfo.tooltipFieldName)} placement='bottom'>
                        {content}
                    </Tooltip>
                );
            }
            return (
                <BodyCell
                    key={index}
                    align={columnInfo.align}
                    className={className}
                    onClick={columnInfo.onClick ? (e => columnInfo.onClick(e, data, title)) : undefined}>
                    {content}
                </BodyCell>
            );
        });
        return (
            <>
                <TableRow className={bodyRowClass ? bodyRowClass : undefined} onClick={this.handleClick}>
                    {cells}
                    {collapseRow ?
                        <BodyCell className={classes.expandCell} align='right'>
                            <IconButton size='small'>
                                {openCollapse ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
                            </IconButton>
                        </BodyCell> : null
                    }
                </TableRow>
                {collapseRow ?
                    <TableRow>
                        <TableCell className={classes.collapseCell} colSpan={columnsInfo.length + 1}>
                            <Collapse in={openCollapse} timeout='auto' unmountOnExit>
                                <Box margin={1}>
                                    {collapseRow}
                                </Box>
                            </Collapse>
                        </TableCell>
                    </TableRow> : null
                }
            </>
        );
    }
};

export default withStyles(styles)(Row);