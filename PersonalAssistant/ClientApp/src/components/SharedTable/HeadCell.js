import { withStyles } from '@material-ui/core/styles';
import TableCell from '@material-ui/core/TableCell';

const HeadCell = withStyles({
    root: {
        padding: '8px',
        fontSize: '1rem'
    },
})(TableCell);

export default HeadCell;