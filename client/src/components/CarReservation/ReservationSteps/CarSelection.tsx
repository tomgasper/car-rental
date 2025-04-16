import { Label } from "@/components/ui/label"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"
import { TESLA_MODELS } from "@/constants/reservation"

interface CarSelectionProps {
  value: string
  onChange: (value: string) => void
}

export function CarSelection({ value, onChange }: CarSelectionProps) {
  return (
    <div className="space-y-4">
      <Label>Select Tesla Model</Label>
      <RadioGroup value={value} onValueChange={onChange} className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {TESLA_MODELS.map((model) => (
          <div key={model.id} className="relative">
            <RadioGroupItem value={model.id} id={model.id} className="peer sr-only" />
            <Label
              htmlFor={model.id}
              className="flex flex-col items-center space-y-2 rounded-md border-2 border-muted bg-popover p-4 hover:bg-accent hover:text-accent-foreground peer-data-[state=checked]:border-primary [&:has([data-state=checked])]:border-primary"
            >
              <img src={model.image || "/placeholder.svg"} alt={model.name} className="h-24 object-contain" />
              <div className="text-center">
                <div className="font-semibold">{model.name}</div>
                <div className="text-sm text-muted-foreground">{model.description}</div>
                <div className="mt-2 font-semibold text-primary">€{model.pricePerDay} / day</div>
              </div>
            </Label>
          </div>
        ))}
      </RadioGroup>
    </div>
  )
}