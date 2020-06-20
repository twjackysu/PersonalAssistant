import React, { Component } from 'react';
import Highcharts from 'highcharts';
import exporting from 'highcharts/modules/exporting';
import HighchartsReact from 'highcharts-react-official';


import { populateData } from '../../helper/CommonFunc';
import HighchartTheme from './HighchartTheme';
exporting(Highcharts);
Highcharts.theme = HighchartTheme;
Highcharts.setOptions(Highcharts.theme);
export default class History extends Component {
    static displayName = History.name;

    constructor(props) {
        super(props);
        this.state = {
            historyBalance: [],
        };
    }
    async setData() {
        let historyBalanceTask = populateData('/api/GetHistoryBalance');
        let historyBalance = await historyBalanceTask;
        this.setState({
            historyBalance: historyBalance
        });
    }
    componentDidMount() {
        this.setData();
    }
    render() {
        let { historyBalance } = this.state;
        let seriesObj = {};
        let monthIndex = {};
        Object.keys(historyBalance).forEach((month, index)=> {
            if(!monthIndex.hasOwnProperty(month)){
                monthIndex[month] = index;
            }
        });
        for(let month in historyBalance){
            historyBalance[month].forEach(asset => {
                if(!seriesObj.hasOwnProperty(asset.Name)){
                    seriesObj[asset.Name] = [];
                }
                seriesObj[asset.Name].push({x: monthIndex[month], y: asset.Balance, date: asset.Date});
            });
        }
        let options = {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Asset Trending Chart'
            },
            xAxis: {
                categories: Object.keys(historyBalance)
            },
            yAxis: {
                title: {
                    text: 'TWD'
                },
                stackLabels: {
                    enabled: true
                }
            },
            tooltip: {
                headerFormat: '<b>{point.x}</b><br/>',
                pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
            },
            plotOptions: {
                column: {
                    stacking: 'normal'
                }
            },
            series: Object.keys(seriesObj).map(assetName => ({
                name: assetName,
                data: seriesObj[assetName]
            }))
        };
        console.log(options);
        return (<div>
            <HighchartsReact
                highcharts={Highcharts}
                options={options}
            />
        </div>
        );
    }
}
