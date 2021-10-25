import React, { Component } from 'react';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableRow from '@material-ui/core/TableRow';
import TableHead from '@material-ui/core/TableHead';
import TableSortLabel from '@material-ui/core/TableSortLabel';
import TableFooter from '@material-ui/core/TableFooter';

import HeadCell from './HeadCell';
import Row from './Row';

const descendingComparator = (a, b, orderBy) => {
    if (b[orderBy] < a[orderBy]) {
        return -1;
    }
    if (b[orderBy] > a[orderBy]) {
        return 1;
    }
    return 0;
}

const getComparator = (order, orderBy) => {
    return order === 'desc'
        ? (a, b) => descendingComparator(a, b, orderBy)
        : (a, b) => -descendingComparator(a, b, orderBy);
}

const stableSort = (array, comparator) => {
    const stabilizedThis = array.map((el, index) => [el, index]);
    stabilizedThis.sort((a, b) => {
        const order = comparator(a[0], b[0]);
        if (order !== 0) return order;
        return a[1] - b[1];
    });
    return stabilizedThis.map((el) => el[0]);
}

class SharedTable extends Component {
    constructor(props) {
        super(props);
        const { defaultOrderBy, defaultOrder } = props;
        this.state = {
            order: defaultOrder ? defaultOrder : 'desc',
            orderBy: defaultOrderBy
        };
        this.handleRequestSort = this.handleRequestSort.bind(this);
    }
    handleAllCollapse(isOpen){
        const { data } = this.props;
        data.forEach((e, i)=>{
            this.refs['row'+i].handleCollapse(isOpen);
        });
    }
    handleRequestSort(event, property){
        const { orderBy, order } = this.state;
        const isAsc = orderBy === property && order === 'asc';
        
        this.setState({
            order: isAsc ? 'desc' : 'asc',
            orderBy: property
        });
    }
    render() {
        const { order, orderBy } = this.state;
        const { data, title, columnsInfo, enabledSort, collapseInfo, headRowClass, bodyRowClass, tableFooter, tableHead, tableClass } = this.props;
        if (!data)
            return null;
        
        const rows = stableSort(data, getComparator(order, orderBy)).map((rowObj, index) =>
            <Row ref={collapseInfo? 'row'+index: undefined} key={index} data={rowObj} columnsInfo={columnsInfo} title={title} collapseRow={collapseInfo ? collapseInfo.rows[rowObj[collapseInfo.mappingField]] : undefined} bodyRowClass={bodyRowClass} />
        );
        const headRows = columnsInfo.map((columnInfo, index) => {
            if (enabledSort && columnInfo.type !== 'trending') {
                return (
                    <HeadCell
                        key={index}
                        align={columnInfo.align}
                        sortDirection={orderBy === columnInfo.fieldName ? order : false}
                        className={columnInfo.className ? columnInfo.className : undefined} >
                        <TableSortLabel
                            active={orderBy === columnInfo.fieldName}
                            direction={orderBy === columnInfo.fieldName ? order : 'asc'}
                            onClick={e => this.handleRequestSort(e, columnInfo.fieldName)}>
                            {columnInfo.headText}
                        </TableSortLabel>
                    </HeadCell>
                );
            } else {
                return (
                    <HeadCell
                        key={index}
                        align={columnInfo.align}
                        className={columnInfo.className ? columnInfo.className : undefined}>
                        {columnInfo.headText}
                    </HeadCell>
                );
            }
        });
        let tableClassName;
        if(tableClass){
            tableClassName = tableClass;
        }
        return (
            <Table className={tableClassName}>
                <TableHead>
                    {tableHead? tableHead : null}
                    {title ? <TableRow className={headRowClass ? headRowClass : undefined}>
                        <HeadCell align='center' colSpan={collapseInfo ? columnsInfo.length + 1 : columnsInfo.length}>{title}</HeadCell>
                    </TableRow> : null}
                    <TableRow className={headRowClass ? headRowClass : undefined}>
                        {headRows}
                        {collapseInfo ? <HeadCell /> : null}
                    </TableRow>
                </TableHead>
                <TableBody>
                    {rows}
                </TableBody>
                {tableFooter? 
                    <TableFooter>
                        {tableFooter}
                    </TableFooter> : null}
            </Table>
        );
    }
}

export default SharedTable;
