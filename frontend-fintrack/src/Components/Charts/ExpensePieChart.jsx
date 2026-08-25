import {Cell, Pie, PieChart, ResponsiveContainer, Tooltip} from "recharts";

 const COLORS = [
  '#0088FE',
  '#00C49F',
  '#FFBB28',
  '#FF8042',
  '#8884D8',
  '#82CA9D',
  '#FF6699',
  '#7dfe04',
  '#8DD1E1',
  '#f8f81f',
  '#B07AA1',
 ];

 const renderCustomizedLabel = ({
  cx,
  cy,
  midAngle,
  innerRadius,
  outerRadius,
  percent,
}) => {
  const RADIAN = Math.PI / 180;
  const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
  const x = cx + radius * Math.cos(-midAngle * RADIAN);
  const y = cy + radius * Math.sin(-midAngle * RADIAN);

  return (
    <text
      x={x}
      y={y}
      fill="white"
      textAnchor={x > cx ? "start" : "end"}
      dominantBaseline="central"
    >
      {`${(percent * 100).toFixed(0)}%`}
    </text>
  );
};

 export default function ExpensesChart({data, responsive = false}){
    const pieChart = (
      <PieChart width={responsive ? "100%" : 700} height={responsive ? "100%" : 360}>
        <Pie data={data} dataKey='value' nameKey='name' label={renderCustomizedLabel} labelLine={false}>
          {data.map((entry, index)=>(
            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]}/>
          ))}
        </Pie>
        <Tooltip/>
      </PieChart>
    );

    return (
    <div className="expense-pie-chart">
      <div className="expense-pie-chart-plot">
        {responsive ? (
          <ResponsiveContainer width="100%" height="100%">
            {pieChart}
          </ResponsiveContainer>
        ) : pieChart}
      </div>
      <ul className="expense-chart-legend">
        {data.map((entry, index) => (
          <li key={`legend-${entry.name}`}>
            <span
              className="expense-chart-legend-swatch"
              style={{ backgroundColor: COLORS[index % COLORS.length] }}
            />
            <span>{entry.name}</span>
          </li>
        ))}
      </ul>
    </div>
    )
 }