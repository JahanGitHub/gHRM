var Page = {
    Load: function () {
        this.BindEvents();
    },
    BindEvents: function () {
        this.LoadGenderWiseEmployeeChart();
        this.LoadLeaveOverviewChart();
    },
    LoadGenderWiseEmployeeChart: function () {
        var data = GenderCountsArr;
        var ctx = document.getElementById('gender-wise-employee').getContext('2d');
        new Chart(ctx, {
            type: 'pie',
            data: {
                datasets: [{
                    data: data,
                    backgroundColor: ['#ff0000', '#0075db', '#03d100', '#808080']
                }],
                labels: GenderNamesArr
            },
            options: {
                responsive: true,
                legend: { position: 'left' },
                animation: {
                    animateScale: false,
                    animateRotate: true
                }
            }
        });
    },
    LoadLeaveOverviewChart: function () {
        var data = Leave6MonthsCountsArr;
        var ctx = document.getElementById('leave-overview').getContext('2d');
        new Chart(ctx, {
            type: 'bar',
            data: {
                datasets: [{
                    data: data,
                    backgroundColor: '#425df5'
                }],
                labels: Leave6MonthsCountMonthNamesArr
            },
            options: {
                responsive: true,
                legend: { display: false },
                animation: {
                    animateScale: false,
                    animateRotate: true
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            min: 0,
                        }
                    }]
                }
            }
        });
    }
};

$(function () {
    Page.Load();
});