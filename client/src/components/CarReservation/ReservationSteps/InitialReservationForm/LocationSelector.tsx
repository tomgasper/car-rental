import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { useCarRentalData } from "@/hooks/useCarRentalData"

interface LocationSelectorProps {
  label: string
  value: string
  onChange: (value: string) => void
  id: string
}

export function LocationSelector({ label, value, onChange, id }: LocationSelectorProps) {
  const { locations, isLoading } = useCarRentalData()

  return (
    <div className="space-y-2">
      <Label htmlFor={id}>{label}</Label>
      <Select value={value} onValueChange={onChange} disabled={isLoading}>
        <SelectTrigger id={id}>
          <SelectValue placeholder="Select location" />
        </SelectTrigger>
        <SelectContent>
          {locations.map((location) => (
            <SelectItem key={location.id} value={location.id}>
              {location.name}
            </SelectItem>
          ))}
        </SelectContent>
      </Select>
    </div>
  )
}