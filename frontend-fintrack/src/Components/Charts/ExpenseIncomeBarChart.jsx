import {
  Bar,
  BarChart,
  CartesianGrid,
  XAxis,
  YAxis,
  Tooltip,
  Rectangle,
} from "recharts";

export default function ExpenseIncomeBarChart({data}){
    return(
        <BarChart width={600} height={300} data={data}>
            <Bar
                dataKey = "income"
                fill="#38bdf8"
                activeBar={<Rectangle fill="#7dd3fc" stroke="#bae6fd" />}
            />
            <Bar
                dataKey="expense"
                fill="#fb7185"
                activeBar={<Rectangle fill="#fda4af" stroke="#fecdd3" />}
            />
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="name" />
            <YAxis />
            <Tooltip cursor={{ fill: "transparent" }} />
        </BarChart>
    )
}